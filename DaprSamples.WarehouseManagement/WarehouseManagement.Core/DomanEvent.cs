namespace WarehouseManagement.Core
{
    public class DomainEvent
    {
        public Guid EventId { get; private set; }
        public Guid CausationId { get; set; } = Guid.Empty;
        public Guid CorrelationId { get; set; } = Guid.Empty;

        public DomainEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}