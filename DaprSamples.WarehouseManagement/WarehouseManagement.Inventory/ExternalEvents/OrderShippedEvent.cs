using WarehouseManagement.Inventory.Models;

namespace WarehouseManagement.Inventory.ExternalEvents
{
    public class OrderShippedEvent
    {
        public Guid EventId { get; set; }
        public Order Order{ get; set; }
    }
}
