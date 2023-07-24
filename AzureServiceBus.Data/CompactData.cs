namespace AzureServiceBus.Data;

public record CompactData(DateTime Created, string Environment, string[]? Filters, string?[] Fields, string[] Queues, Dictionary<string, string[]?> Topics)
{
    public static CompactData Create(Data data) =>
        new(
            data.Created,
            data.Environment,
            data.Filters,
            data.Fields,
            data.Queues.Select(queue => $"{queue.Name} ({queue.MessageCount} | {queue.DeadLetterMessageCount})").ToArray(),
            data.Topics.ToDictionary(topic => $"{topic.Name}", topic => topic.Subscriptions?.Select(subscription => $"{subscription.Name} ({subscription.MessageCount} | {subscription.DeadLetterMessageCount})").ToArray())
        );
}