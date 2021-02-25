using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using XB.HttpClientJwt.Config;

namespace XB.HttpClientJwt
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;
        private readonly HttpClientJwtOptions _httpClientJwtOptions;

        private string _jwt;
        private long _jwtExpire;

        public AuthenticationDelegatingHandler(ILogger<AuthenticationDelegatingHandler> logger,
            IOptions<HttpClientJwtOptions> config)
        {
            _logger = logger;
            _httpClientJwtOptions = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await DoRequestWithJwt(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _jwt = string.Empty;
                _jwtExpire = 0;

                return await DoRequestWithJwt(request, cancellationToken);
            }

            return response;
        }

        private async Task<HttpResponseMessage> DoRequestWithJwt(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetJwt(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetJwt(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_jwt) || DateTimeOffset.UtcNow.ToUnixTimeSeconds() < _jwtExpire)
            {
                return _jwt;
            }

            var jwtRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_httpClientJwtOptions.Url));

            var jwtDetails = new Dictionary<string, string>
            {
                { "grant_type", _httpClientJwtOptions.Grant_Type },
                { "client_id", _httpClientJwtOptions.ClientId },
                { "client_secret", _httpClientJwtOptions.ClientSecret },
                { "username", _httpClientJwtOptions.Username },
                { "password", _httpClientJwtOptions.Password },
                { "scope", _httpClientJwtOptions.Scope },
            };

            jwtRequest.Content = new FormUrlEncodedContent(jwtDetails);

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var response = await base.SendAsync(jwtRequest, cancellationToken);

            var content = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

            _jwt = content?.SelectToken("access_token")?.ToString();
            _jwtExpire = long.Parse(content?.SelectToken("expires_in")?.ToString() ?? "0", CultureInfo.InvariantCulture)
                         + currentTime;

            return _jwt;
        }
    }
}
