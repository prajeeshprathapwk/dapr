namespace WarehouseManagement.Customers.Models
{
    public class Address
    {
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string Extension { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}