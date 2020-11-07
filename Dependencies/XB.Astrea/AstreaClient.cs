using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XB.Astrea.Client;

namespace XB.Astrea
{
    public class AstreaClient : IAstreaClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AstreaClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AstreaResponse> AssessAsync(string mt)
        {
            var request = AstreaRequestHelper.ParseMtToAstreaRequest(mt);

            var data = new StringContent(request.Mt, Encoding.UTF8, "text/plain");

            var result = await _httpClientFactory.CreateClient("astrea").PostAsync("/swift", data);
            //var result = await _httpClientFactory.CreateClient("astrea").PostAsync("/sas/v3/assessOrders/paymentInstruction", data);

            return JsonConvert.DeserializeObject<AstreaResponse>(await result.Content.ReadAsStringAsync());
        }
    }
}
