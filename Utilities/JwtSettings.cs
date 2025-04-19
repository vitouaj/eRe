public class JwtSettings
{
    public string IssuerSigningKey { get; set; }
    public string ValidIssuer { get; set; }
    public string[] ValidAudiences { get; set; }
}