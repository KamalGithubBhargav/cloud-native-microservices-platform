namespace CloudNative.ConfigLibrary.Interfaces
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(string topic, string message);
    }
}
