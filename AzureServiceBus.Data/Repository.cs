using Azure.Messaging.ServiceBus.Administration;

namespace AzureServiceBus.Data;

public class Repository
{
    protected readonly ServiceBusAdministrationClient ServiceBusAdministrationClient;

    protected Repository(string connectionString)
    {
        ServiceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString);
    }
}