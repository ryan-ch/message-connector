using System;
using System.Threading.Tasks;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;

namespace XB.Hubert
{
    public interface IHubertClient
    {
        Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResultAsync(string timestamp, string guid, string transactionStatus, int rowId = 1, string sourceId = "SWIFT");
    }
}
