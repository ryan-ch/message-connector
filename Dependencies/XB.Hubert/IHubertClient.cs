using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using System.Threading.Tasks;

namespace XB.Hubert
{
    public interface IHubertClient
    {
        Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResultAsync(string timestamp, string id, string transactionStatus);
    }
}
