using AutoMapper;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Customers.Events;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Commands
{
    public class ShipOrderCommand : IRequest<Order>
    {
        public ShipOrderCommand(Order order)
        {
            Ensure.That(order).IsNotNull();
            Order = order;
        }

        public Order Order { get; }
    }

    public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, Order>
    {
        private readonly ILogger<ShipOrderCommandHandler> logger;
        private readonly IOrderService service;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ShipOrderCommandHandler(ILogger<ShipOrderCommandHandler> logger, IOrderService service, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.service = service;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Order> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
        {
            await service.ProcessOrderAsync(request.Order.OrderId, cancellationToken);
            logger.LogCommand<ShipOrderCommand>(request);

            var @event = mapper.Map<OrderShippedEvent>(request);
            await mediator.Publish<OrderShippedEvent>(@event, cancellationToken);
            return request.Order;
        }
    }
}
