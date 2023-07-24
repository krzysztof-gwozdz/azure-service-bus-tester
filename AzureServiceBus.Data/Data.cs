namespace AzureServiceBus.Data;

public record Data(DateTime Created, string Environment, string[]? Filters, Queue[] Queues, Topic[] Topics);