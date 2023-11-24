namespace WarehouseManagement.Orders.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public OrderState State { get; set; } = OrderState.New;
    }

    public enum OrderState
    {
        New,
        Cancelled,
        Shipped
    }
}
