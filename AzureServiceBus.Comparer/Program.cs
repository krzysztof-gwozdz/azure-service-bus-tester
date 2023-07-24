using System.Text.Json;
using AzureServiceBus;
using AzureServiceBus.Data;

var before =  await Deserialize("C:/bannerflow/before.json");
var after =  await Deserialize("C:/bannerflow/after.json");

async Task<Data?> Deserialize(string fileName) => JsonSerializer.Deserialize<Data>(await File.ReadAllTextAsync(fileName));

