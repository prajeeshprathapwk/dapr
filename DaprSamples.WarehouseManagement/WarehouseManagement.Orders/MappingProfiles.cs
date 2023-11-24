using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.ExternalEvents;
using AutoMapper;
using WarehouseManagement.Customers.Events;
using WarehouseManagement.Orders.Queries;
using WarehouseManagement.Orders.Events;

namespace WarehouseManagement.Orders.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CustomerCreatedEvent, CreateCustomerCommand>()
                .ForMember(_ => _.Customer, opt => opt.MapFrom(_ => _.Customer));
            CreateMap<InventoryUpdatedEvent, UpdateInventoryCommand>()
                .ForMember(_ => _.Inventory, opt => opt.MapFrom(_ => _.Inventory));
            CreateMap<Order, CreateOrderCommand>()
                .ConstructUsing(_ => new CreateOrderCommand(_));
            CreateMap<CreateOrderCommand, OrderCreatedEvent>()
                .ConstructUsing(_ => new OrderCreatedEvent(_.Order));
            CreateMap<OrderCreatedEvent, CancelOrderCommand>()
                .ConstructUsing(_ => new CancelOrderCommand(_.Order));
            CreateMap<CancelOrderCommand, OrderCancelledEvent>()
                .ConstructUsing(_ => new OrderCancelledEvent(_.Order, _.Reason));
            CreateMap<OrderCreatedEvent, ShipOrderCommand>()
                .ConstructUsing(_ => new ShipOrderCommand(_.Order));
            CreateMap<ShipOrderCommand, OrderShippedEvent>()
                .ConstructUsing(_ => new OrderShippedEvent(_.Order));
            CreateMap<Tuple<Order, Inventory>, ProductOrder>()
                .ForMember(_ => _.Product, opt => opt.MapFrom(_ => _.Item2.ProductName))
                .ForMember(_ => _.OrderId, opt => opt.MapFrom(_ => _.Item1.OrderId))
                .ForMember(_ => _.Quantity, opt => opt.MapFrom(_ => _.Item1.Quantity))
                .ForMember(_ => _.Price, opt => opt.MapFrom(_ => _.Item2.UnitPrice * _.Item1.Quantity))
                .ForMember(_ => _.State, opt => opt.MapFrom(_ => Enum.GetName(typeof(OrderState), _.Item1.State)));
        }
    }
}
