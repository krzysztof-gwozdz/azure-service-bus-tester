namespace AzureServiceBus.Data;

public class TopicsRepository : Repository
{
    public TopicsRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<Topic[]> Get(string[]? filters, string[]? fields, CancellationToken cancellationToken)
    {
        var topics = new List<Topic>();
        await foreach (var topicRuntimeProperties in ServiceBusAdministrationClient.GetTopicsRuntimePropertiesAsync(cancellationToken))
        {
            if (filters is not null && filters.All(filter => topicRuntimeProperties.Name.ToLower().Contains(filter)))
                topics.Add(Topic.Create(topicRuntimeProperties, await GetSubscriptions(topicRuntimeProperties.Name, fields), fields));
        }

        return topics.ToArray();
    }

    private async Task<Subscription[]> GetSubscriptions(string topicPath, string[]? fields)
    {
        var subscriptions = new List<Subscription>();
        await foreach (var subscriptionRuntimeProperties in ServiceBusAdministrationClient.GetSubscriptionsRuntimePropertiesAsync(topicPath))
        {
            var subscriptionDescription = await ManagementClient.GetSubscriptionAsync(topicPath, subscriptionRuntimeProperties.SubscriptionName);
            subscriptions.Add(Subscription.Create(subscriptionRuntimeProperties, subscriptionDescription, fields));
        }

        return subscriptions.ToArray();
    }
}