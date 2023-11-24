using Dapr.Client;
using EnsureThat;
using MediatR;
using System.Runtime.Versioning;
using WarehouseManagement.Core;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers.Events
{
    public class CustomerCreatedEvent : DomainEvent, INotification
    {
        public CustomerCreatedEvent(Customer customer) : base()
        {
            Ensure.That(customer).IsNotNull();
            Customer = customer;
        }

        public Customer Customer { get; set; }
    }

    public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly string PUBSUB_NAME = "pubsub";
        private readonly string TOPIC_NAME = "customers";
        private readonly DaprClient dapr;
        private readonly ILogger<CustomerCreatedEventHandler> logger;

        public CustomerCreatedEventHandler(DaprClient dapr, ILogger<CustomerCreatedEventHandler> logger)
        {
            this.dapr = dapr;
            this.logger = logger;
        }

        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            await dapr.PublishIntegrationEvent<CustomerCreatedEvent>(PUBSUB_NAME, TOPIC_NAME, notification, cancellationToken: cancellationToken);
            logger.LogEventPublished(notification);
        }
    }
}
