namespace AzureServiceBus.Data;

public record Data(DateTime Created, Configuration Configuration, Queue[] Queues, Topic[] Topics);