using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBus.Data;

public record Subscription(string Name, string ForwardTo, long MessageCount, long DeadLetterMessageCount)
{
    public static Subscription Create(SubscriptionRuntimeProperties subscriptionRuntimeProperties, SubscriptionDescription subscriptionDescription) =>
        new(subscriptionDescription.SubscriptionName, subscriptionDescription.ForwardTo, subscriptionRuntimeProperties.ActiveMessageCount, subscriptionRuntimeProperties.DeadLetterMessageCount);
}