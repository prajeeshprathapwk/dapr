using Dapr.Client;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;

namespace WarehouseManagement.Inventory.Events
{
    public class InventoryUpdatedEvent : DomainEvent, INotification
    {
        public InventoryUpdatedEvent(Models.Inventory inventory) : base()
        {
            Ensure.That(inventory).IsNotNull();
            Inventory = inventory;
        }

        public Models.Inventory Inventory { get; set; }
    }

    public class InventoryUpdatedEventHandler : INotificationHandler<InventoryUpdatedEvent>
    {
        private readonly string PUBSUB_NAME = "pubsub";
        private readonly string TOPIC_NAME = "inventories";
        private readonly DaprClient dapr;
        private readonly ILogger<InventoryUpdatedEventHandler> logger;

        public InventoryUpdatedEventHandler(DaprClient dapr, ILogger<InventoryUpdatedEventHandler> logger)
        {
            this.dapr = dapr;
            this.logger = logger;
        }

        public async Task Handle(InventoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await dapr.PublishIntegrationEvent<InventoryUpdatedEvent>(PUBSUB_NAME, TOPIC_NAME, notification, cancellationToken: cancellationToken);
            logger.LogEventPublished(notification);
        }
    }
}
