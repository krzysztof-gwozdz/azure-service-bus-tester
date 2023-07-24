using System.Diagnostics;
using System.Text.Json;
using AzureServiceBus.Data;
using Microsoft.Azure.ServiceBus.Management;

var connectionString = new Dictionary<string, string>
{
    ["test"] = "",
    ["dev"] = "",
    ["sandbox"] = "",
    ["prod"] = ""
};
var environment = "sandbox";
var managementClient = new ManagementClient(connectionString[environment]);
var queuesRepository = new QueuesRepository(managementClient);
var topicsRepository = new TopicsRepository(managementClient);

using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));

var filters = new[] { "tiktok" };

var data = new Data(
    DateTime.Now,
    environment,
    filters,
    await queuesRepository.Get(filters, cancellationTokenSource.Token),
    await topicsRepository.Get(filters, cancellationTokenSource.Token)
);

var fileName = environment;
var filePath = $"C:/bannerflow/{fileName}.json";
var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(filePath, json);
Process.Start("C:/Program Files/Microsoft VS Code/Code.exe", filePath);