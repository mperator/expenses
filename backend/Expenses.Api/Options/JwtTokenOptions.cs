namespace Expenses.Api.Options
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
