namespace AzureServiceBus.Data;

public class Repository
{
    protected async Task<List<T>> GetAll<T>(Func<int, int, CancellationToken, Task<IList<T>>> getFunc, CancellationToken cancellationToken)
    {
        var items = new List<T>();
        const int count = 100;
        var skip = 0;
        bool continueLoop;
        do
        {
            var queues = (await getFunc(count, skip, cancellationToken)).ToArray();
            items.AddRange(queues);
            skip += count;
            continueLoop = queues.Length == count;
        } while (continueLoop);

        return items.ToList();
    }
}