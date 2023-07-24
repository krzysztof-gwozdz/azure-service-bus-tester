using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBus.Data;

public class QueuesRepository : Repository
{
    private readonly ManagementClient _managementClient;

    public QueuesRepository(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    public async Task<Queue[]> Get(string[]? filters, CancellationToken cancellationToken)
    {
        var queueDescriptions = await GetAll((count, skip, _) => _managementClient.GetQueuesAsync(count, skip, cancellationToken), cancellationToken);
        if (filters is not null)
            queueDescriptions = queueDescriptions.Where(queueDescription => filters.All(filter => queueDescription.Path.ToLower().Contains(filter))).ToList();

        return queueDescriptions.Select(queueDescription => new Queue(queueDescription.Path)).ToArray();
    }
}