using System;

namespace IMS.Business.DTOs;

public class LoginResultDto
{
    public required string AccessToken { get; set; }

    public required string UserId { get; set; }

    public required string UserInfo { get; set; }

    public required DateTime ExpiresAt { get; set; }

    public required DateTime IssuedAt { get; set; }
}
