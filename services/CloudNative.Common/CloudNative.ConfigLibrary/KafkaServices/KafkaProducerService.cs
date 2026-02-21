using CloudNative.ConfigLibrary.Constants;
using CloudNative.ConfigLibrary.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace CloudNative.ConfigLibrary.KafkaServices
{
    public class KafkaProducerService: IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IConfiguration _config;
        public KafkaProducerService(IConfiguration config)
        {
            _config = config;
            string bootstrapServers = _config[KafkaConstant.BootstrapServers]!;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                EnableIdempotence = true,
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, string message)
        {
            var result = await _producer.ProduceAsync(
            topic, new Message<Null, string>
            { Value = message });
            Console.WriteLine($"[Producer] Sent: { message} to { result.TopicPartitionOffset }");
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}
