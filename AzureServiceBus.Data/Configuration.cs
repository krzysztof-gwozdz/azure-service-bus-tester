namespace AzureServiceBus.Data;

public record Configuration
{
    public string FileName { get; init; } = null!;
    public string Environment { get; init; } = null!;
    public string[]? Filters { get; init; }
    public string[]? Fields { get; init; }
}