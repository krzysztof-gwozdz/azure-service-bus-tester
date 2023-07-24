using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public record Topic(string Path, Subscription[] Subscriptions)
{
    public static Topic Create(TopicProperties topicProperties, Subscription[] subscriptions) =>
        new(topicProperties.Name, subscriptions);
}