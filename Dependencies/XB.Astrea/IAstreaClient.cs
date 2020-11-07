using System.Threading.Tasks;
using XB.Astrea.Client.Assessment;

namespace XB.Astrea.Client
{
    public interface IAstreaClient
    {
        Task<Response> AssessAsync(string mt);
    }
}
