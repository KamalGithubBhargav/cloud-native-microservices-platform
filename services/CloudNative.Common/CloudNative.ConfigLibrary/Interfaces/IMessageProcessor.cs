namespace CloudNative.ConfigLibrary.Interfaces
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(string message, CancellationToken cancellationToken);
    }
}
