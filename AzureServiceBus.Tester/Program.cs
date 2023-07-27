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

var configuration = new Configuration
{
    FileName = "before",
    Environment = "test",
    Filters = null, //new[] { "insta" },
    Fields = null, //new[] { "queue.name", "topic.name", "topic.subscriptions", "subscription.name" },
    NotEmpty = false
};

var data = await GetData();
SaveData();

async Task<Data> GetData()
{
    var connectionString = connectionStrings[configuration.Environment];
    var queuesRepository = new QueuesRepository(connectionString);
    var topicsRepository = new TopicsRepository(connectionString);
    using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));

    return new Data(
        DateTime.Now,
        configuration,
        await queuesRepository.Get(configuration, cancellationTokenSource.Token),
        await topicsRepository.Get(configuration, cancellationTokenSource.Token)
    );
}

void SaveData()
{
    var filePath = $"C:/bannerflow/{configuration.FileName}.json";
    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });
    File.WriteAllText(filePath, json);
    Process.Start("C:/Program Files/Microsoft VS Code/Code.exe", filePath);
}