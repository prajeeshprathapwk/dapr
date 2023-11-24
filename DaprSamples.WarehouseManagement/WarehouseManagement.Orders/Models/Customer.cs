namespace WarehouseManagement.Orders.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Address Address { get; set; }
    }
}
