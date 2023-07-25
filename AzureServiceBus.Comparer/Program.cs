using System.Text.Json;
using AzureServiceBus.Data;

var before = (await Deserialize("C:/bannerflow/prod.json"))!;
var after = (await Deserialize("C:/bannerflow/after.json"))!;

async Task<Data?> Deserialize(string fileName) => JsonSerializer.Deserialize<Data>(await File.ReadAllTextAsync(fileName));


var differences = new List<Difference>();

differences.AddRange(
    before.Queues
        .Where(queue => after.Queues.All(x => x.Name != queue.Name))
        .Select(queue => new Difference($"[-] Queue {queue.Name}"))
);

differences.AddRange(
    after.Queues
        .Where(queue => before.Queues.All(x => x.Name != queue.Name))
        .Select(queue => new Difference($"[+] Queue {queue.Name}"))
);

differences.AddRange(
    before.Topics
        .Where(topic => after.Topics.All(x => x.Name != topic.Name))
        .Select(topic => new Difference($"[-] Topic {topic.Name}"))
);

differences.AddRange(
    after.Topics
        .Where(topic => before.Topics.All(x => x.Name != topic.Name))
        .Select(topic => new Difference($"[+] Topic {topic.Name}"))
);


foreach (var difference in differences)
{
    if (difference.Text.Contains("[-]"))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(difference.Text);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(difference.Text);
    }
}
Console.ResetColor();


public record Difference(string Text);