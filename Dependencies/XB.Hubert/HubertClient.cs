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
        private readonly ILogger<HubertClient> _logger;


        public HubertClient(IHttpClientFactory httpClientFactory, IOptions<HubertClientOptions> config, ILogger<HubertClient> logger)
        {
            _config = config.Value;
            _httpClient = httpClientFactory.CreateClient("sebcs");
            _httpClient.BaseAddress = new Uri(_config.Url);
            _logger = logger;
        }

        public async Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResultAsync(string timestamp, string guid, string transactionStatus, int rowId = 0, string sourceId = "SWIFT")
        {
            var csUpdate01Fcpsts01SimpleClient =
                new CrossbordpmtUpdate01Fcpsts01SimpleClient(_httpClient);

            var request = new CrossbordpmtUpdate01Fcpsts01Request
            {
                Data = new CrossbordpmtUpdate01Fcpsts01Tables
                {
                    Uakw4630 = new UAKW4630
                    {
                        CreateTimestamp = timestamp,
                        Guid = guid,
                        RowId = rowId,
                        SourceId = sourceId,
                        TransactionStatus = transactionStatus
                    }
                }
            };

            return await csUpdate01Fcpsts01SimpleClient.PostAsync(null, null, null, _config.ClientId, request);
        }
    }
}