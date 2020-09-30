namespace XB.IBM.MQ
{
    public interface IMqProducer
    {
        void Start();

        void WriteMessage(string message);
    }
}
