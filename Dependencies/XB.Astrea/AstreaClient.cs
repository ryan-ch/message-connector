using System.Net.Http;
using System.Threading.Tasks;

namespace XB.Astrea
{
    public class AstreaClient : IAstreaClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public AstreaClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("astrea");
        }

        public async Task<string> SayHelloAsync()
        {
            return await _httpClient.GetStringAsync("/sas/v3/hello");
        }
    }
}
