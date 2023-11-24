using AutoMapper;
using WarehouseManagement.Inventory.Commands;
using WarehouseManagement.Inventory.Events;
using WarehouseManagement.Inventory.ExternalEvents;
using WarehouseManagement.Inventory.Models;

namespace WarehouseManagement.Inventory
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Models.Inventory, AddInventoryCommand>()
                .ConstructUsing(_ => new AddInventoryCommand(_));
            CreateMap<OrderShippedEvent, UpdateInventoryCommand>()
                .ConstructUsing(_ => new UpdateInventoryCommand(_.Order.ProductId, _.Order.Quantity));
            CreateMap<Models.Inventory, InventoryUpdatedEvent>()
                .ConstructUsing(_ => new InventoryUpdatedEvent(_));
            CreateMap<AddInventoryCommand, InventoryUpdatedEvent>()
                .ConstructUsing(_ => new InventoryUpdatedEvent(_.Inventory));
        }
    }
}
