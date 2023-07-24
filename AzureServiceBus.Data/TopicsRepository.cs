namespace AzureServiceBus.Data;

public class TopicsRepository : Repository
{
    public TopicsRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<Topic[]> Get(string[]? filters, CancellationToken cancellationToken)
    {
        var topics = new List<Topic>();
        await foreach (var topicRuntimeProperties in ServiceBusAdministrationClient.GetTopicsRuntimePropertiesAsync(cancellationToken))
        {
            if (filters is not null && filters.All(filter => topicRuntimeProperties.Name.ToLower().Contains(filter)))
                topics.Add(new Topic(topicRuntimeProperties.Name, await GetSubscriptions(topicRuntimeProperties.Name)));
        }

        return topics.ToArray();
    }

    private async Task<Subscription[]> GetSubscriptions(string topicPath)
    {
        var subscriptions = new List<Subscription>();
        await foreach (var subscriptionRuntimeProperties in ServiceBusAdministrationClient.GetSubscriptionsRuntimePropertiesAsync(topicPath))
        {
            var subscriptionDescription = await ManagementClient.GetSubscriptionAsync(topicPath, subscriptionRuntimeProperties.SubscriptionName);
            subscriptions.Add(new Subscription(
                subscriptionRuntimeProperties.SubscriptionName,
                subscriptionDescription.ForwardTo[(subscriptionDescription.ForwardTo.IndexOf("net/", StringComparison.InvariantCultureIgnoreCase) + 4)..],
                subscriptionRuntimeProperties.ActiveMessageCount,
                subscriptionRuntimeProperties.DeadLetterMessageCount));
        }

        return subscriptions.ToArray();
    }
}