
namespace IMS.API.Config;

public class AppSetting
{
    public required Jwt Jwt { get; set; }
}

public class Jwt
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public required string Secret { get; set; }
    public required string ExpirationInMinutes { get; set; }
}
