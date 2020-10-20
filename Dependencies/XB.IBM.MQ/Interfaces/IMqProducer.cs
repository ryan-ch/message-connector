using System.Threading.Tasks;

namespace XB.IBM.MQ.Interfaces
{
    public interface IMqProducer
    {
        void WriteMessage(string message);

        void Commit();

        void Rollback();
    }
}
