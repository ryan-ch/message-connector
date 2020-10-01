namespace XB.IBM.MQ
{
    public interface IMqConsumer
    {
        void Start();

        string ReceiveMessage();

        void Commit();
    }
}
