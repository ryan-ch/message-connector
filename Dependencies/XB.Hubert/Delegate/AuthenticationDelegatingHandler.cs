using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using XB.Hubert.Config;

namespace XB.Hubert.Delegate
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;
        private readonly HubertClientOptions _hubertClientOptions;

        private string _jwt;
        private long _jwtExpire;

        public AuthenticationDelegatingHandler(ILogger<AuthenticationDelegatingHandler> logger,
            IOptions<HubertClientOptions> config)
        {
            _logger = logger;
            _hubertClientOptions = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetJwt(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                token = await GetJwt(cancellationToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await base.SendAsync(request, cancellationToken);

                return response;
            }

            return response;
        }

        private async Task<string> GetJwt(CancellationToken cancellationToken)
        {
            if (_jwt == null || DateTimeOffset.UtcNow.ToUnixTimeSeconds() > _jwtExpire)
            {
                var jwtRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(GetUrlToJwt()));

                var response = await base.SendAsync(jwtRequest, cancellationToken);

                var content = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

                _jwt = content?.SelectToken("access_token")?.ToString();
                _jwtExpire = long.Parse(content?.SelectToken("expires_in")?.ToString() ?? "0", CultureInfo.InvariantCulture);

                _logger.LogInformation(
                    $"Successfully fetched an new JWT for user {_hubertClientOptions.JwtClientId}.");

                return _jwt;
            }

            _logger.LogInformation(
                $"Successfully re-used JWT for user {_hubertClientOptions.JwtClientId}.");

            return _jwt;
        }

        private string GetUrlToJwt()
        {
            return _hubertClientOptions.JwtUrl + $"?grant_typ=password&client_id={_hubertClientOptions.JwtClientId}&client_secret={_hubertClientOptions.JwtClientSecret}&username={_hubertClientOptions.Username}&password={_hubertClientOptions.Password}&scope=openid";
        }
    }
}
