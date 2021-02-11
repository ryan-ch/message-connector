using System.Threading.Tasks;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;

namespace XB.Hubert
{
    public interface IHubertClient
    {
        Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResponse();
    }
}
