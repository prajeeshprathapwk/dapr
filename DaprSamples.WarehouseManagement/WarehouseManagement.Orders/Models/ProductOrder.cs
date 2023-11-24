namespace WarehouseManagement.Orders.Models
{
    public class ProductOrder
    {
        public Guid OrderId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string State { get; set; }
    }
}
