using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBus.Data;

public class TopicsRepository : Repository
{
    private readonly ManagementClient _managementClient;

    public TopicsRepository(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    public async Task<Topic[]> Get(string[]? filters, CancellationToken cancellationToken)
    {
        var topicDescriptions = await GetAll((count, skip, _) => _managementClient.GetTopicsAsync(count, skip, cancellationToken), cancellationToken);
        if (filters is not null)
            topicDescriptions = topicDescriptions.Where(topicDescription => filters.All(filter => topicDescription.Path.ToLower().Contains(filter))).ToList();
        var topics = new List<Topic>();
        foreach (var topicDescription in topicDescriptions)
        {
            topics.Add(new Topic(
                topicDescription.Path,
                await GetSubscriptions(topicDescription.Path)
            ));
        }

        return topics.ToArray();
    }

    private async Task<Subscription[]> GetSubscriptions(string topicPath)
    {
        var subscriptionDescriptions = await _managementClient.GetSubscriptionsAsync(topicPath);
        return subscriptionDescriptions
            .Select(subscriptionDescription => new Subscription(subscriptionDescription.SubscriptionName, subscriptionDescription.ForwardTo.Substring(subscriptionDescription.ForwardTo.IndexOf("net/", StringComparison.InvariantCultureIgnoreCase) + 4)))
            .ToArray();
    }
}