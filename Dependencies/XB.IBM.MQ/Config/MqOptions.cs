namespace XB.IBM.MQ.Config
{
    public record MqOptions
    {
        public const string ConfigurationSection = "MqConfigurations";
        // Todo: Are these different?
        // If not then we don't need 2 instances from the MqBase, maybe a singeleton?
        public MqConfigurations ReaderConfig { get; init; }
        public MqConfigurations WriterConfig { get; init; }
    }

    public record MqConfigurations
    {
        public string MqChannel { get; init; }
        public string MqQueueManagerName { get; init; }
        public string MqQueueName { get; init; }
        public string MqHostname { get; init; }
        public int MqPort { get; init; }
        public string MqSslPath { get; init; }
        public string MqSslCipher { get; init; }
        public string MqPeerName { get; init; }
        public string MqUserName { get; init; }
        public string MqPassword { get; init; }
    }
}
