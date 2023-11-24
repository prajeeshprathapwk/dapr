using Dapr.Client;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.ExternalEvents
{
    public class InventoryUpdatedEvent : DomainEvent, INotification
    {
        public Inventory Inventory { get; set; }
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
            await dapr.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, notification, cancellationToken: cancellationToken);
            logger.LogEventPublished(notification);
        }
    }
}
