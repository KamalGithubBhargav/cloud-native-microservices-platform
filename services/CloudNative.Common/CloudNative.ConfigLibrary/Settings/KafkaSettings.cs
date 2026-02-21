using CloudNative.ConfigLibrary.Constants;
using Microsoft.Extensions.Configuration;

namespace CloudNative.ConfigLibrary.Settings
{
    public class KafkaSettings
    {
        private readonly IConfiguration _config;
        public KafkaSettings(IConfiguration config)
        {
            _config = config;
        }

        public string BootstrapServers
        {
            get { return _config[KafkaConstant.BootstrapServers]!; }
        }
    }
}
