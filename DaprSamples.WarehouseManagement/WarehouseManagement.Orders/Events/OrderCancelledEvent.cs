using Dapr.Client;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Customers.Events
{
    public class OrderCancelledEvent : DomainEvent, INotification
    {
        public OrderCancelledEvent(Order order, string reason) : base()
        {
            Ensure.That(order).IsNotNull();
            Order = order;
            Reason = reason;
        }

        public Order Order { get; private set; }
        public string Reason { get; private set; }
    }

    public class OrderCancelledEventHandler : INotificationHandler<OrderCancelledEvent>
    {
        private readonly string PUBSUB_NAME = "pubsub";
        private readonly string TOPIC_NAME = "orders";
        private readonly DaprClient dapr;
        private readonly ILogger<OrderCancelledEventHandler> logger;
        private readonly IOrderService orderService;

        public OrderCancelledEventHandler(DaprClient dapr, 
            ILogger<OrderCancelledEventHandler> logger, 
            IOrderService orderService, 
            IMediator mediator)
        {
            this.dapr = dapr;
            this.logger = logger;
            this.orderService = orderService;
        }

        public async Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
        {
            await orderService.CancelOrderAsync(notification.Order.OrderId, cancellationToken);
            await dapr.PublishIntegrationEvent<OrderCancelledEvent>(PUBSUB_NAME, TOPIC_NAME, notification, cancellationToken: cancellationToken);
            logger.LogEventPublished(notification);
        }
    }
}
