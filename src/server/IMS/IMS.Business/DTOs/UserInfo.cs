namespace IMS.Business.DTOs;

public class UserInfo
{
    public required int Id { get; set; }

    public required string Username { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public required string[] Roles { get; set; }
}