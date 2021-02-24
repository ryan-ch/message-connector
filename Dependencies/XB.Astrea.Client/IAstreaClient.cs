using System.Threading.Tasks;
using XB.Astrea.Client.Messages.Assessment;

namespace XB.Astrea.Client
{
    public interface IAstreaClient
    {
        Task<AssessmentResponse> AssessAsync(string mt);
    }
}
