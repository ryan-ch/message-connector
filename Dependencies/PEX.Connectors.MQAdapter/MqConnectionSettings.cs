namespace PEX.Connectors.MQAdapter
{
    public class MqConnectionSettings : IMqConnectionSettings
    {
        public string Channel { get; set; }
        public string QueueManagerName { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public int PollInterval { get; set; }
        public string SslPath { get; set; }
        public string SslChyper { get; set; }
        public bool SetIdentityContext { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
