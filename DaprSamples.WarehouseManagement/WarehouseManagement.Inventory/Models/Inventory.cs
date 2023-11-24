namespace WarehouseManagement.Inventory.Models
{
    public class Inventory
    {
        public Inventory()
        {
            ProductId = Guid.NewGuid();
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
