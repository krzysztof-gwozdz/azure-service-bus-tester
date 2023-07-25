using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public record Subscription(string? Name, string? ForwardTo, long? MessageCount, long? DeadLetterMessageCount)
{
    public static Subscription Create(SubscriptionRuntimeProperties subscriptionRuntimeProperties, SubscriptionProperties subscriptionProperties, string[]? fields) =>
        new(
            fields is null || fields.Contains("subscription.name") ? subscriptionRuntimeProperties.SubscriptionName : null,
            fields is null || fields.Contains("subscription.forwardTo") ? subscriptionProperties.ForwardTo[(subscriptionProperties.ForwardTo.IndexOf("net/", StringComparison.InvariantCultureIgnoreCase) + 4)..] : null,
            fields is null || fields.Contains("subscription.messageCount") ? subscriptionRuntimeProperties.ActiveMessageCount : null,
            fields is null || fields.Contains("subscription.deadLetterMessageCount") ? subscriptionRuntimeProperties.DeadLetterMessageCount : null
        );
}