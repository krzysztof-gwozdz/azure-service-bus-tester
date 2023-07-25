namespace AzureServiceBus.Data;

public class TopicsRepository : Repository
{
    public TopicsRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<Topic[]> Get(Configuration configuration, CancellationToken cancellationToken)
    {
        var topics = new List<Topic>();
        await foreach (var topicRuntimeProperties in ServiceBusAdministrationClient.GetTopicsRuntimePropertiesAsync(cancellationToken))
        {
            if (configuration.Filters is not null && configuration.Filters.All(filter => topicRuntimeProperties.Name.ToLower().Contains(filter)))
                topics.Add(Topic.Create(topicRuntimeProperties, await GetSubscriptions(topicRuntimeProperties.Name, configuration.Fields), configuration.Fields));
        }

        return topics.ToArray();
    }

    private async Task<Subscription[]> GetSubscriptions(string topicPath, string[]? fields)
    {
        var subscriptions = new List<Subscription>();
        await foreach (var subscriptionRuntimeProperties in ServiceBusAdministrationClient.GetSubscriptionsRuntimePropertiesAsync(topicPath))
        {
            var subscriptionProperties = await ServiceBusAdministrationClient.GetSubscriptionAsync(topicPath, subscriptionRuntimeProperties.SubscriptionName);
            subscriptions.Add(Subscription.Create(subscriptionRuntimeProperties, subscriptionProperties, fields));
        }

        return subscriptions.ToArray();
    }
}