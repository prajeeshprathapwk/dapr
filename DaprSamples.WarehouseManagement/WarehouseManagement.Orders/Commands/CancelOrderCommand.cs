using AutoMapper;
using Dapr.Client;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Customers.Events;
using WarehouseManagement.Orders.ExternalEvents;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Commands
{
    public class CancelOrderCommand : IRequest<Order>
    {
        public CancelOrderCommand(Order order)
        {
            Ensure.That(order).IsNotNull();
            Order = order;
        }

        public Order Order { get; }
        public string Reason { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Order>
    {
        private readonly ILogger<CancelOrderCommandHandler> logger;
        private readonly IOrderService service;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CancelOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger, IOrderService service, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.service = service;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Order> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            await service.CancelOrderAsync(request.Order.OrderId, cancellationToken);
            logger.LogCommand<CancelOrderCommand>(request);

            var @event = mapper.Map<OrderCancelledEvent>(request);
            await mediator.Publish<OrderCancelledEvent>(@event, cancellationToken);
            return request.Order;
        }
    }
}
