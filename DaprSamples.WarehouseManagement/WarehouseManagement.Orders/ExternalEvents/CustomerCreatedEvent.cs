using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.ExternalEvents
{
    public class CustomerCreatedEvent
    {
        public Guid EventId { get; set; }
        public Customer Customer { get; set; }
    }
}
