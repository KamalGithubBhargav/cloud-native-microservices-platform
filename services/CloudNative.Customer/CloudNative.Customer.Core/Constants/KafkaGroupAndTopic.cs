namespace CloudNative.Customer.Core.Constants
{
    public class KafkaGroupAndTopic
    {
        public string Topic { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
    }

    public static class KafkaRegistry
    {
        public static readonly List<KafkaGroupAndTopic> Consumers = new()
        {
            new KafkaGroupAndTopic
            {
                Topic = "customer-topic",
                Group = "customer-group"
            },
            new KafkaGroupAndTopic
            {
                Topic = "order-topic",
                Group = "order-group"
            },
            new KafkaGroupAndTopic
            {
                Topic = "auth-topic",
                Group = "auth-group"
            }
        };
    }
}
