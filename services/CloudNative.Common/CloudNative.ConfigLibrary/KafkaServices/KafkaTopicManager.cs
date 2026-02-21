using CloudNative.ConfigLibrary.Constants;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;

namespace CloudNative.ConfigLibrary.KafkaServices
{
    public class KafkaTopicManager

    {
        private readonly AdminClientConfig _config;
        private readonly IConfiguration _globConfig;

        public KafkaTopicManager(IConfiguration config)
        {
            _globConfig = config;
            string bootstrapServers = _globConfig[KafkaConstant.BootstrapServers]!;
            _config = new AdminClientConfig { BootstrapServers = bootstrapServers };
        }

        public async Task CreateTopicIfNotExistsAsync(string topic, int partitions = 1,
        short replication = 1)
        {
            using var adminClient = new AdminClientBuilder(_config).Build();
            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                if (!metadata.Topics.Any(t => t.Topic == topic))
                {
                    var newTopic = new TopicSpecification
                    {
                        Name = topic,
                        NumPartitions = partitions,
                        ReplicationFactor = replication
                    };
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification>                    {
                        newTopic });
                    Console.WriteLine($"[Admin] Topic & '{topic}' created.");
                }
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                    Console.WriteLine($"[Admin] Error creating topic: { e.Results[0].Error.Reason}");
            }
        }
    }
}
