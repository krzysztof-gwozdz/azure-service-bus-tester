namespace AzureServiceBus.Data;

public record Queue(string Path, long MessageCount, long DeadLetterMessageCount);