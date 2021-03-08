using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using XB.HttpClientJwt.Config;

namespace XB.HttpClientJwt
{
    //Todo: should this be moved in to common or keep it in own library?
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private static string _jwt;
        private static long _jwtExpire;
        private readonly HttpClientJwtOptions _httpClientJwtOptions;
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;

        public AuthenticationDelegatingHandler(IOptions<HttpClientJwtOptions> config, ILogger<AuthenticationDelegatingHandler> logger)
        {
            _logger = logger;
            _httpClientJwtOptions = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await DoRequestWithJwt(request, cancellationToken);
            if (response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Forbidden)
                return response;

            _jwt = string.Empty;
            _jwtExpire = 0;

            return await DoRequestWithJwt(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> DoRequestWithJwt(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await GetJwt(cancellationToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to generate JWT for requests");
                throw;
            }
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetJwt(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_jwt) && DateTimeOffset.UtcNow.ToUnixTimeSeconds() < _jwtExpire)
                return _jwt;

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
            var response = await base.SendAsync(jwtRequest, cancellationToken);
            var content = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

            _jwt = content.SelectToken("access_token")?.ToString();
            if (string.IsNullOrWhiteSpace(_jwt))
                throw new Exception("Unexpected empty token");

            _jwtExpire = long.Parse(content.SelectToken("expires_in")?.ToString() ?? "0", CultureInfo.InvariantCulture) + DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return _jwt;
        }
    }
}
