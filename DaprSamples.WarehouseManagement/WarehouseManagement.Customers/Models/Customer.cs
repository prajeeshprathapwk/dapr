namespace WarehouseManagement.Customers.Models
{
    public class Customer
    {
        public Customer()
        {
            CustomerId = Guid.NewGuid();
        }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Address Address { get; set; }
    }
}
