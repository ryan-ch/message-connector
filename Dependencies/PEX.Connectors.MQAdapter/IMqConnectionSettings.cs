namespace PEX.Connectors.MQAdapter
{
    public interface IMqConnectionSettings
    {
        string Channel { get; }
        string QueueManagerName { get; }
        string Hostname { get; }
        int Port { get; }
        int PollInterval { get; }
        string SslPath { get; }
        string SslChyper { get; }
        bool SetIdentityContext { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}