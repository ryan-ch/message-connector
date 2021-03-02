namespace XB.HttpClientJwt.Config
{
    public record HttpClientJwtOptions
    {
        public const string ConfigurationSection = "HttpClientJwt";

        public string Url { get; set; }
        public string Grant_Type { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Scope { get; set; }
        //Todo: recommended to use init instead of set
    }
}
