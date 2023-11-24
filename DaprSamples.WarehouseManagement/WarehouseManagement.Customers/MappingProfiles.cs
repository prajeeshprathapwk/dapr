using AutoMapper;
using WarehouseManagement.Customers.Commands;
using WarehouseManagement.Customers.Events;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CreateCustomerCommand>()
                .ConstructUsing(_ => new CreateCustomerCommand(_));
            CreateMap<CreateCustomerCommand, CustomerCreatedEvent>()
                .ConstructUsing(_ => new CustomerCreatedEvent(_.Customer));
        }
    }
}
