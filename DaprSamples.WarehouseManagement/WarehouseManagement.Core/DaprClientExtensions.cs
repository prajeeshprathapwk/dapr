using Dapr.Client;

namespace WarehouseManagement.Core
{
    public static class DaprClientExtensions
    {
        public static async Task PublishIntegrationEvent<T>(this DaprClient dapr, string pubSubName, string topicName, T payload, CancellationToken cancellationToken)
        {
            var integrationEvent = new IntegrationEvent<T>();
            integrationEvent.Payload = payload;
            integrationEvent.Type = typeof(T).Name;

            await dapr.PublishEventAsync<IntegrationEvent<T>>(pubSubName, topicName, integrationEvent, cancellationToken);
        }
    }
}
