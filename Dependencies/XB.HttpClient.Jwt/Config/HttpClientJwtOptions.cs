namespace XB.HttpClientJwt.Config
{
    public record HttpClientJwtOptions
    {
        public const string ConfigurationSection = "HttpClientJwt";

        public string Url { get; init; }
        public string Grant_Type { get; init; }
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Scope { get; init; }
    }
}
