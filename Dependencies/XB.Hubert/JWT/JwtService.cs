using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace XB.Hubert.JWT
{
    public class JwtService
    {
        private readonly string user;
        private static HttpClient jwtHttpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://jwks-stage.cumulus.sebank.se")
        };
        private string jwt;
        private long jwtExpiry;

        public JwtService(string user)
        {
            this.user = user;
        }

        public async Task<string> GetJWT()
        {
            if (this.jwt == null || DateTimeOffset.UtcNow.ToUnixTimeSeconds() > this.jwtExpiry)
            {
                //In a real application, another method should be used...this is just for testing/demonstration.
                HttpResponseMessage response = await jwtHttpClient.GetAsync($"api/token?user={this.user}");
                this.jwt = await response.Content.ReadAsStringAsync();
                JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedJwt = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
                string expiryUnix = decodedJwt.Claims.First(c => c.Type == "exp").Value;
                this.jwtExpiry = Int64.Parse(expiryUnix);
                Console.WriteLine($"Successfully fetched an new JWT for user {this.user}.");
            }
            else
                Console.WriteLine($"Using existing JWT for user {this.user}.");
            Console.WriteLine($"JWT expiry(UTC): { DateTimeOffset.FromUnixTimeSeconds(jwtExpiry)}, current UTC time: { DateTimeOffset.UtcNow}");
            return jwt;
        }
    }
}
