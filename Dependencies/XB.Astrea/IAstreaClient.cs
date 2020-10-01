using System.Threading.Tasks;

namespace XB.Astrea
{
    public interface IAstreaClient
    {
        Task<bool> Assess(string mt);
    }
}
