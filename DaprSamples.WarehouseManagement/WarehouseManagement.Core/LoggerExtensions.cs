using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WarehouseManagement.Core
{
    public static class LoggerExtensions
    {
        public static void LogDomainEvent<T>(this ILogger logger, T data)
        {
            logger.Log(LogLevel.Information, "Event: {Type}, Name: {Event}, Payload: {Payload}", "DomainEvent", typeof(T).Name, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void LogEventPublished<T>(this ILogger logger, T data)
        {
            logger.Log(LogLevel.Information, "Event: {Type}, Name: {Event}, Payload: {Payload}", "EventProducer", typeof(T).Name, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void LogEventSubscribed<T>(this ILogger logger, T data)
        {
            logger.Log(LogLevel.Information, "Event: {Type}, Name: {Event}, Payload: {Payload}", "EventSubscriber", typeof(T).GetIntegrationEventName(), JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void LogCommand<T>(this ILogger logger, T data)
        {
            logger.Log(LogLevel.Information, "Command: {Type}, Name: {Command}, Payload: {Payload}", "Command", typeof(T).Name, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        
        public static void LogQuery<T, TK>(this ILogger logger, TK data)
        {
            logger.Log(LogLevel.Information, "Query: {Type}, Name: {Query}, Result : {Result}", "Query", typeof(T).Name, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
