using AutoMapper;
using Dapr.Client;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Events
{
    public class OrderCreatedEvent : DomainEvent, INotification
    {
        public OrderCreatedEvent(Order order) : base()
        {
            Ensure.That(order).IsNotNull();
            Order = order;
        }

        public Order Order { get; set; }
    }

    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly string PUBSUB_NAME = "pubsub";
        private readonly string TOPIC_NAME = "orders";
        private readonly DaprClient dapr;
        private readonly ILogger<OrderCreatedEventHandler> logger;
        private readonly IOrderService orderService;
        private readonly IInventoryService inventoryService;
        private readonly ICustomerService customerService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public OrderCreatedEventHandler(DaprClient dapr,
            ILogger<OrderCreatedEventHandler> logger,
            IOrderService orderService,
            IInventoryService inventoryService,
            ICustomerService customerService,
            IMediator mediator,
            IMapper mapper)
        {
            this.dapr = dapr;
            this.logger = logger;
            this.orderService = orderService;
            this.inventoryService = inventoryService;
            this.customerService = customerService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var customer = await customerService.GetByIdAsync(notification.Order.CustomerId, cancellationToken);
            if (customer == null)
            {
                var cancelOrderCommand = mapper.Map<CancelOrderCommand>(notification);
                cancelOrderCommand.Reason = $"A customer with id {notification.Order.CustomerId} not found";
                await mediator.Send(cancelOrderCommand, cancellationToken);
                return;
            }
            var inventory = await inventoryService.GetByIdAsync(notification.Order.ProductId, cancellationToken);
            if (inventory == null)
            {
                var cancelOrderCommand = mapper.Map<CancelOrderCommand>(notification);
                cancelOrderCommand.Reason = $"Product with id {notification.Order.ProductId} not found in inventory";
                await mediator.Send(cancelOrderCommand, cancellationToken);
                return;
            }
            if (inventory.Quantity < notification.Order.Quantity)
            {
                var cancelOrderCommand = mapper.Map<CancelOrderCommand>(notification);
                cancelOrderCommand.Reason = $"Insufficient Inventory";
                await mediator.Send(cancelOrderCommand, cancellationToken);
                return;
            }

            var shipOrderCommand = mapper.Map<ShipOrderCommand>(notification);
            await mediator.Send(shipOrderCommand, cancellationToken);
        }
    }
}
