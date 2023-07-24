using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public record Queue(string? Name, long? MessageCount, long? DeadLetterMessageCount)
{
    public static Queue Create(QueueRuntimeProperties queueRuntimeProperties, string[]? fields) =>
        new(
            fields is null || fields.Contains("queue.name") ? queueRuntimeProperties.Name : null,
            fields is null || fields.Contains("queue.messageCount") ? queueRuntimeProperties.ActiveMessageCount : null,
            fields is null || fields.Contains("queue.deadLetterMessageCount") ? queueRuntimeProperties.DeadLetterMessageCount : null
        );
}