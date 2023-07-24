namespace AzureServiceBus.Data;

public record Data(DateTime Created, string Environment, string[]? Filters, string?[] Fields, Queue[] Queues, Topic[] Topics);