namespace WarehouseManagement.Core
{
    public class IntegrationEvent<TEvent>
    {
        public TEvent Payload { get; set; }
        public string Type { get; set; }
    }
}
