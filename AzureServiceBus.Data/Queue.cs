using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public record Queue(string? Path, long? MessageCount, long? DeadLetterMessageCount)
{
    public static Queue Create(QueueRuntimeProperties queueRuntimeProperties) => 
        new(queueRuntimeProperties.Name, queueRuntimeProperties.ActiveMessageCount, queueRuntimeProperties.DeadLetterMessageCount);
}