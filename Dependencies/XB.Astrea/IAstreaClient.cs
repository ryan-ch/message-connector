using System.Threading.Tasks;
using XB.Astrea.Client;

namespace XB.Astrea
{
    public interface IAstreaClient
    {
        Task<AstreaResponse> AssessAsync(string mt);
    }
}
