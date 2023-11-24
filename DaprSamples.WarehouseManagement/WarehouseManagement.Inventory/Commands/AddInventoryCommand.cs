using AutoMapper;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Inventory.Events;

namespace WarehouseManagement.Inventory.Commands
{
    public class AddInventoryCommand : IRequest<Models.Inventory>
    {
        public AddInventoryCommand(Models.Inventory inventory)
        {
            Ensure.That(inventory).IsNotNull();
            Inventory = inventory;
        }

        public Models.Inventory Inventory { get; }
    }

    public class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, Models.Inventory>
    {
        private readonly ILogger<AddInventoryCommandHandler> logger;
        private readonly IInventoryService service;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddInventoryCommandHandler(ILogger<AddInventoryCommandHandler> logger, IInventoryService service, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.service = service;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Models.Inventory> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            await service.AddAsync(request.Inventory, cancellationToken);
            logger.LogCommand(request);

            var @event = mapper.Map<InventoryUpdatedEvent>(request);
            await mediator.Publish<InventoryUpdatedEvent>(@event, cancellationToken);
            return request.Inventory;
        }
    }
}
