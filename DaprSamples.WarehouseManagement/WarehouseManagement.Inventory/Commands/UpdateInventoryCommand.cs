using AutoMapper;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Inventory.Events;

namespace WarehouseManagement.Inventory.Commands
{
    public class UpdateInventoryCommand : IRequest<Models.Inventory>
    {
        public UpdateInventoryCommand(Guid productId, int quantity)
        {
            Ensure.That(productId).IsNotEmpty();
            Ensure.That(quantity).IsGt(0);
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid ProductId { get; }
        public int Quantity { get; }
    }

    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Models.Inventory>
    {
        private readonly ILogger<UpdateInventoryCommandHandler> logger;
        private readonly IInventoryService service;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public UpdateInventoryCommandHandler(ILogger<UpdateInventoryCommandHandler> logger, IInventoryService service, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.service = service;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Models.Inventory> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = await service.UpdateAsync(request.ProductId, request.Quantity, cancellationToken);
            logger.LogCommand(request);

            var @event = mapper.Map<InventoryUpdatedEvent>(inventory);
            await mediator.Publish<InventoryUpdatedEvent>(@event, cancellationToken);
            return inventory;
        }
    }
}
