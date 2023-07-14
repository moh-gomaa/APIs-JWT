namespace ApisJwt.Helpers
{
    public class JwtInfo
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string DurationInDays { get; set; } = null!;
    }
}
