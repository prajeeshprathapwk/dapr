using Dapr.Client;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Customers.Events
{
    public class OrderShippedEvent : DomainEvent, INotification
    {
        public OrderShippedEvent(Order order) : base()
        {
            Ensure.That(order).IsNotNull();
            Order = order;
        }

        public Order Order { get; set; }
    }

    public class OrderShippedEventHandler : INotificationHandler<OrderShippedEvent>
    {
        private readonly string PUBSUB_NAME = "pubsub";
        private readonly string TOPIC_NAME = "ometopic";
        private readonly DaprClient dapr;
        private readonly ILogger<OrderShippedEventHandler> logger;

        public OrderShippedEventHandler(DaprClient dapr, 
            ILogger<OrderShippedEventHandler> logger)
        {
            this.dapr = dapr;
            this.logger = logger;
        }

        public async Task Handle(OrderShippedEvent notification, CancellationToken cancellationToken)
        {
            await dapr.PublishIntegrationEvent<OrderShippedEvent>(PUBSUB_NAME, TOPIC_NAME, notification, cancellationToken: cancellationToken);
            logger.LogEventPublished(notification);
        }
    }
}
