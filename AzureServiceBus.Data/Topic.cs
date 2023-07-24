using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public record Topic(string? Name, Subscription[]? Subscriptions)
{
    public static Topic Create(TopicRuntimeProperties topicRuntimeProperties, Subscription[]? subscriptions, string[]? fields) =>
        new(
            fields is null || fields.Contains("topic.name") ? topicRuntimeProperties.Name : null,
            fields is null || fields.Contains("topic.subscriptions") ? subscriptions : null
        );
}