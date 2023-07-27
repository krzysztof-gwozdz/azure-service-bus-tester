public record Difference(Change Change, Type Type, string Value);

public enum Change
{
    Added,
    Removed
}

public enum Type
{
    Queue,
    Topic,
    Subscription
}