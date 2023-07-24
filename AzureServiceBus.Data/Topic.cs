namespace AzureServiceBus.Data;

public record Topic(string Path, Subscription[] Subscriptions);