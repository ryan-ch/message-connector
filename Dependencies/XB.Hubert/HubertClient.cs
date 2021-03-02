using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using XB.Hubert.Config;

namespace XB.Hubert
{
    public class HubertClient : IHubertClient
    {
        private readonly HubertClientOptions _config;
        private readonly HttpClient _httpClient;


        public HubertClient(IHttpClientFactory httpClientFactory, IOptions<HubertClientOptions> config)
        {
            _config = config.Value;
            _httpClient = httpClientFactory.CreateClient(HubertClientOptions.HttpClientIdentifier);
            _httpClient.BaseAddress = new Uri(_config.Url);
        }

        public async Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResultAsync(string timestamp, string guid, string transactionStatus)
        {
            var hubertServiceClient =
                new CrossbordpmtUpdate01Fcpsts01SimpleClient(_httpClient);

            var request = new CrossbordpmtUpdate01Fcpsts01Request
            {
                Data = new CrossbordpmtUpdate01Fcpsts01Tables
                {
                    Uakw4630 = new UAKW4630
                    {
                        CreateTimestamp = timestamp,
                        Guid = guid,
                        RowId = 1,
                        SourceId = "SWIFT",
                        TransactionStatus = transactionStatus
                    }
                }
            };

            return await hubertServiceClient.PostAsync(null, null, null, _config.ClientId, request);
        }
    }
}