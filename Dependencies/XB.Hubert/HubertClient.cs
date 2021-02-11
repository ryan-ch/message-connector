using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SEB.SEBCS.RTM.v1.Client.Uakm463;
using SEB.SEBCS.RTM.v1.Client.Uakm463.Crossbordpmt.Update01.Fcpsts01;
using XB.Hubert.Config;
using XB.Hubert.JWT;

namespace XB.Hubert
{
    public class HubertClient : IHubertClient
    {
        private readonly HubertClientOptions _config;
        private readonly HttpClient _httpClient;

        public HubertClient(IHttpClientFactory httpClientFactory, IOptions<HubertClientOptions> config)
        {
            _config = config.Value;
            _httpClient = httpClientFactory.CreateClient(HubertExtensions.HttpClientName);
        }

        public async Task<CrossbordpmtUpdate01Fcpsts01Response> SendAssessmentResponse(string timestamp, string guid, string transactionStatus, int rowId =1, string sourceId ="TEST")
        {
			JwtService jwtService = new JwtService("s4556c");

            CrossbordpmtUpdate01Fcpsts01SimpleClient csUpdate01Fcpsts01SimpleClient =
                new CrossbordpmtUpdate01Fcpsts01SimpleClient(_httpClient);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await jwtService.GetJWT());

            CrossbordpmtUpdate01Fcpsts01Request request = new CrossbordpmtUpdate01Fcpsts01Request()
            {
                Data = new CrossbordpmtUpdate01Fcpsts01Tables()
                {
                    Uakw4630 = new UAKW4630()
                    {
                        CreateTimestamp = timestamp ,//"2020-02-08 08:35:00.000000",
                        Guid = guid,//"ced0f305-f722-4855-bfc7-5da3bf38bebc",
                        RowId = rowId,//1,
                        SourceId = sourceId,//"TEST",
                        TransactionStatus = transactionStatus//"7"
                    }
                }
            };

            return await csUpdate01Fcpsts01SimpleClient.PostAsync(null, null, null, _config.ClientId, request);
        }
    }
}