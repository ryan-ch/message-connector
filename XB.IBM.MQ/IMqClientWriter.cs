namespace XB.IBM.MQ
{
    public interface IMqClientWriter
    {
        void Start();

        void WriteMessage(string message);
    }
}
