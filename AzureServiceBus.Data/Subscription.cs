namespace AzureServiceBus.Data;

public record Subscription(string Name, string ForwardTo, long MessageCount, long DeadLetterMessageCount);