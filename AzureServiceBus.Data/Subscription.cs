using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBus.Data;

public record Subscription(string? Name, string? ForwardTo, long? MessageCount, long? DeadLetterMessageCount)
{
    public static Subscription Create(SubscriptionRuntimeProperties subscriptionRuntimeProperties, SubscriptionDescription subscriptionDescription, string[]? fields) =>
        new(
            fields is null || fields.Contains("subscription.name") ? subscriptionRuntimeProperties.SubscriptionName : null,
            fields is null || fields.Contains("subscription.forwardTo") ? subscriptionDescription.ForwardTo[(subscriptionDescription.ForwardTo.IndexOf("net/", StringComparison.InvariantCultureIgnoreCase) + 4)..] : null,
            fields is null || fields.Contains("subscription.messageCount") ? subscriptionRuntimeProperties.ActiveMessageCount : null,
            fields is null || fields.Contains("subscription.deadLetterMessageCount") ? subscriptionRuntimeProperties.DeadLetterMessageCount : null
        );
}