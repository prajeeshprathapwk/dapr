using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Commands
{
    public class UpdateInventoryCommand : IRequest<Inventory>
    {
        public Inventory Inventory { get; set;  }
    }

    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Inventory>
    {
        private readonly ILogger<UpdateInventoryCommandHandler> logger;
        private readonly IInventoryService service;

        public UpdateInventoryCommandHandler(ILogger<UpdateInventoryCommandHandler> logger, IInventoryService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public async Task<Inventory> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            await service.AddAsync(request.Inventory, cancellationToken);
            logger.LogCommand(request);
            return request.Inventory;
        }
    }
}
