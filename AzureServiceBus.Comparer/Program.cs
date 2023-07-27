using System.Text.Json;
using AzureServiceBus.Data;

var before = (await Deserialize("C:/bannerflow/before2.json"))!;
var after = (await Deserialize("C:/bannerflow/after14.json"))!;

async Task<Data?> Deserialize(string fileName) => JsonSerializer.Deserialize<Data>(await File.ReadAllTextAsync(fileName));


var differences = new List<Difference>();

AddDifferences(after, before, Change.Added);
AddDifferences(before, after, Change.Removed);
DisplayResults();

void AddDifferences(Data data1, Data data2, Change change)
{
    //queues
    differences.AddRange(
        data1.Queues
            .Where(queue1 => data2.Queues.All(queue2 => queue2.Name != queue1.Name))
            .Select(queue => new Difference(change, Type.Queue, queue.Name!))
    );

    //topics
    differences.AddRange(
        data1.Topics
            .Where(topic1 => data2.Topics.All(topic2 => topic2.Name != topic1.Name))
            .Select(topic => new Difference(change, Type.Topic, $"{topic.Name!}{string.Join("|", topic.Subscriptions?.Select(s => s.Name) ?? ArraySegment<string?>.Empty) }"))
    );

    //subscriptions
    foreach (var topic1 in data1.Topics.Where(topic1 => data2.Topics.Any(topic2 => topic2.Name == topic1.Name)))
    {
        var subscriptions1 = topic1.Subscriptions;
        var topic2 = data2.Topics.FirstOrDefault(topic => topic.Name == topic1.Name);
        if (topic2 is null)
            continue;
        var subscriptions2 = topic2.Subscriptions;
        differences.AddRange(
            subscriptions1
                .Where(subscription1 => subscriptions2.All(subscription2 => subscription2.Name != subscription1.Name))
                .Select(subscription => new Difference(change, Type.Subscription, $"{topic1.Name} --- {subscription.Name!}"))
        );
    }
}

void DisplayResults()
{
    differences = differences.OrderBy(difference => difference.Type).ThenBy(difference => difference.Value).ToList();
    foreach (var difference in differences)
    {
        Console.ForegroundColor = difference.Change switch
        {
            Change.Added => ConsoleColor.Green,
            Change.Removed => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
        Console.WriteLine($"[{difference.Type}] {difference.Value}");
    }

    Console.ResetColor();
}