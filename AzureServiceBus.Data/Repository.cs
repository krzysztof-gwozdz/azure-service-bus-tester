using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Azure.ServiceBus.Management;

namespace AzureServiceBus.Data;

public class Repository
{
    protected readonly ManagementClient ManagementClient;
    protected readonly ServiceBusAdministrationClient ServiceBusAdministrationClient;

    protected Repository(string connectionString)
    {
        ManagementClient = new ManagementClient(connectionString);
        ServiceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString);
    }
}