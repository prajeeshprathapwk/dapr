using AutoMapper;
using EnsureThat;
using MediatR;
using WarehouseManagement.Orders.Models;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Events;

namespace WarehouseManagement.Orders.Commands
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public CreateOrderCommand(Order order)
        {
            Ensure.That(order).IsNotNull();
            order.State = OrderState.New;
            Order = order;
        }

        public Order Order { get; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly ILogger<CreateOrderCommandHandler> logger;
        private readonly IOrderService service;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrderService service, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.service = service;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await service.AddAsync(request.Order, cancellationToken);
            logger.LogCommand<CreateOrderCommand>(request);

            var @event = mapper.Map<OrderCreatedEvent>(request.Order);
            await mediator.Publish(@event, cancellationToken);
            return request.Order;
        }
    }
}
