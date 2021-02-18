namespace XB.HttpClientJwt.Config
{
    public class HttpClientJwtOptions
    {
        public const string ConfigurationSection = "HttpClientJwt";

        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
