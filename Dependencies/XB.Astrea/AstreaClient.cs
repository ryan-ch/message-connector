using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task<AstreaResponse> Assess(string mt)
        {
            var data = new StringContent(mt, Encoding.UTF8, "text/plain");

            var result = await _httpClientFactory.CreateClient("astrea").PostAsync("/swift", data);

            return JsonSerializer.Deserialize<AstreaResponse>(await result.Content.ReadAsStringAsync());
        }
    }
}
