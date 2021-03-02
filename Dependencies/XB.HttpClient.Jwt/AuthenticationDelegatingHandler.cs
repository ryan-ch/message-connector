using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using XB.HttpClientJwt.Config;

namespace XB.HttpClientJwt
{
    //Todo: should this be moved in to common or keep it in own library?
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        private string _jwt;
        private long _jwtExpire;

        //public AuthenticationDelegatingHandler(IOptions<HttpClientJwtOptions> config)
        public AuthenticationDelegatingHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            //_httpClientJwtOptions = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await DoRequestWithJwt(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Forbidden)
            {
                return response;
            }

            _jwt = string.Empty;
            _jwtExpire = 0;

            return await DoRequestWithJwt(request, cancellationToken);
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

            //var jwtRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_httpClientJwtOptions.Url));
            var jwtRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration.GetValue<string>("AppSettings:HttpClientJwt:Url")));

            var jwtDetails = new Dictionary<string, string>
            {
                { "grant_type", _configuration.GetValue<string>("AppSettings:HttpClientJwt:Grant_Type") },
                { "client_id", _configuration.GetValue<string>("AppSettings:HttpClientJwt:ClientId")},
                { "client_secret", _configuration.GetValue<string>("AppSettings:HttpClientJwt:ClientSecret") },
                { "username", _configuration.GetValue<string>("AppSettings:HttpClientJwt:Username") },
                { "password", _configuration.GetValue<string>("AppSettings:HttpClientJwt:Password") },
                { "scope", _configuration.GetValue<string>("AppSettings:HttpClientJwt:openid") },
            };

            jwtRequest.Content = new FormUrlEncodedContent(jwtDetails);

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var response = await base.SendAsync(jwtRequest, cancellationToken);

            var content = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

            //Todo: content is never null so do we need a null check here?
            _jwt = content?.SelectToken("access_token")?.ToString();
            _jwtExpire = long.Parse(content?.SelectToken("expires_in")?.ToString() ?? "0", CultureInfo.InvariantCulture)
                         + currentTime;

            return _jwt;
        }
    }
}
