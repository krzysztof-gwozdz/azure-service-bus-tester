namespace AzureServiceBus.Data;

public class QueuesRepository : Repository
{
    public QueuesRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<Queue[]> Get(string[]? filters, CancellationToken cancellationToken)
    {
        var queues = new List<Queue>();
        await foreach (var queueRuntimeProperties in ServiceBusAdministrationClient.GetQueuesRuntimePropertiesAsync(cancellationToken))
        {
            if (filters is not null && filters.All(filter => queueRuntimeProperties.Name.ToLower().Contains(filter)))
                queues.Add(new Queue(queueRuntimeProperties.Name, queueRuntimeProperties.ActiveMessageCount, queueRuntimeProperties.DeadLetterMessageCount));
        }
        return queues.ToArray();
    }
}