namespace XB.IBM.MQ.Interfaces
{
    public interface IMqConsumer
    {
        string ReceiveMessage();
        void Commit();
        void Rollback();
    }
}
