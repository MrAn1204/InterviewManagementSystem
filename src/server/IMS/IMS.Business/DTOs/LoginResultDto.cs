using System;

namespace IMS.Business.DTOs;

public class LoginResultDto
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public required UserInfo UserInfo { get; set; }

    public required DateTime ExpiresAt { get; set; }
}
