using Microsoft.Extensions.Options;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using XB.Hubert.Config;

namespace XB.Hubert
{
    public class HubertClient : IHubertClient
    {
        private const string HubertDateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
        private readonly string _clientId;
        private readonly HttpClient _httpClient;


        public HubertClient(IHttpClientFactory httpClientFactory, IOptions<HubertClientOptions> config)
        {
            _clientId = config.Value.ClientId;
            _httpClient = httpClientFactory.CreateClient(HubertClientOptions.HttpClientIdentifier);
            _httpClient.BaseAddress = new Uri(config.Value.Url);
        }

        public async Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResultAsync(DateTime timestamp, string id, string transactionStatus)
        {
            var hubertServiceClient = new CrossbordpmtUpdate01Fcpsts01SimpleClient(_httpClient);

            var request = new CrossbordpmtUpdate01Fcpsts01Request
            {
                Data = new CrossbordpmtUpdate01Fcpsts01Tables
                {
                    Uakw4630 = new UAKW4630
                    {
                        CreateTimestamp = timestamp.ToString(HubertDateTimeFormat),
                        Guid = id,
                        RowId = 1,
                        SourceId = "SWIFT",
                        TransactionStatus = transactionStatus
                    }
                }
            };

            return await hubertServiceClient.PostAsync(null, null, null, _clientId, request);
        }
    }
}