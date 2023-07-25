namespace AzureServiceBus.Data;

public class QueuesRepository : Repository
{
    public QueuesRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<Queue[]> Get(Configuration configuration, CancellationToken cancellationToken)
    {
        var queues = new List<Queue>();
        await foreach (var queueRuntimeProperties in ServiceBusAdministrationClient.GetQueuesRuntimePropertiesAsync(cancellationToken))
        {
            if ((configuration.Filters is null || configuration.Filters.All(filter => queueRuntimeProperties.Name.ToLower().Contains(filter)))
                && (!configuration.NotEmpty || queueRuntimeProperties.ActiveMessageCount + queueRuntimeProperties.DeadLetterMessageCount > 0))
                queues.Add(Queue.Create(queueRuntimeProperties, configuration.Fields));
        }

        return queues.ToArray();
    }
}