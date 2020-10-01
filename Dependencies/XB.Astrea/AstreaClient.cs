using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XB.Astrea
{
    public class AstreaClient : IAstreaClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AstreaClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Assess(string mt)
        {
            var data = new StringContent(mt, Encoding.UTF8, "application/json");

            var result = await _httpClientFactory.CreateClient("astrea").PostAsync("/sas/v3/assessOrders/paymentInstruction", data);

            return result.IsSuccessStatusCode;
        }
    }
}
