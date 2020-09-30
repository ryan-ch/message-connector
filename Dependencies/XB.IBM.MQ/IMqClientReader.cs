namespace XB.IBM.MQ
{
    public interface IMqClientReader
    {
        void Start();

        string ReceiveMessage();
    }
}
