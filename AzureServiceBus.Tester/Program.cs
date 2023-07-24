using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AzureServiceBus.Data;


var connectionStrings = new Dictionary<string, string>
{
    ["test"] = "",
    ["dev"] = "",
    ["sandbox"] = "",
    ["prod"] = ""
};
var environment = "sandbox";
var connectionString = connectionStrings[environment];

var queuesRepository = new QueuesRepository(connectionString);
var topicsRepository = new TopicsRepository(connectionString);
using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));

var filters = new[] { "tiktok" };
var fields = new[] { "queue.name", "topic.name", "topic.subscriptions", "subscription.name" };
var data = new Data(
    DateTime.Now,
    environment,
    filters,
    fields,
    await queuesRepository.Get(filters, fields, cancellationTokenSource.Token),
    await topicsRepository.Get(filters, fields, cancellationTokenSource.Token)
);

var fileName = environment;
var filePath = $"C:/bannerflow/{fileName}.json";
var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
File.WriteAllText(filePath, json);
Process.Start("C:/Program Files/Microsoft VS Code/Code.exe", filePath);