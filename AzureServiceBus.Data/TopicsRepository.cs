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
            if (configuration.Filters is null || configuration.Filters.All(filter => topicRuntimeProperties.Name.ToLower().Contains(filter)))
            {
                var subscriptions = await GetSubscriptions(configuration, topicRuntimeProperties.Name);
                if (subscriptions.Any())
                    topics.Add(Topic.Create(topicRuntimeProperties, subscriptions, configuration.Fields));
            }
        }

        return topics.ToArray();
    }

    private async Task<Subscription[]> GetSubscriptions(Configuration configuration, string topicPath)
    {
        var subscriptions = new List<Subscription>();
        await foreach (var subscriptionRuntimeProperties in ServiceBusAdministrationClient.GetSubscriptionsRuntimePropertiesAsync(topicPath))
        {
            var subscriptionProperties = await ServiceBusAdministrationClient.GetSubscriptionAsync(topicPath, subscriptionRuntimeProperties.SubscriptionName);
            if (!configuration.NotEmpty || subscriptionRuntimeProperties.ActiveMessageCount + subscriptionRuntimeProperties.DeadLetterMessageCount > 0)
                subscriptions.Add(Subscription.Create(subscriptionRuntimeProperties, subscriptionProperties, configuration.Fields));
        }

        return subscriptions.ToArray();
    }
}