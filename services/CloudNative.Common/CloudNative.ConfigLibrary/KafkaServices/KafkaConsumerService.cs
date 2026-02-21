using CloudNative.ConfigLibrary.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace CloudNative.ConfigLibrary.KafkaServices
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly string _groupId;
        private readonly IMessageProcessor _processor;

        public KafkaConsumerService(
            string bootstrapServers,
            string topic,
            string groupId,
            IMessageProcessor processor) // inject your custom processor
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
            _groupId = groupId;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                Acks = Acks.All
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            consumer.Subscribe(_topic);
            Console.WriteLine("[Consumer] Listening...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result == null)
                    {
                        continue;
                    }

                    bool success = false;
                    int retryCount = 0;
                    const int maxRetries = 5;

                    while (!success && retryCount < maxRetries)
                    {
                        try
                        {
                            Console.WriteLine($"[Consumer] Processing: {result.Message.Value}");

                            await _processor.ProcessAsync(result.Message.Value, stoppingToken);

                            success = true;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            Console.WriteLine($"[Retry {retryCount}] {ex.Message}");

                            if (retryCount < maxRetries)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)), stoppingToken);
                            }
                        }
                    }

                    if (success)
                    {
                        consumer.Commit(result);
                    }
                    else
                    {
                        Console.WriteLine("[DLT] Sending message to dead-letter topic...");
                        await producer.ProduceAsync(_topic + "-dlt", new Message<Null, string>
                        {
                            Value = result.Message.Value
                        }, stoppingToken);
                        consumer.Commit(result);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer shutting down...");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}