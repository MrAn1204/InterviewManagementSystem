using System;

namespace IMS.Domain.Entities;

public class ResetPasswordToken : BaseEntity
{
    public required string Token { get; set; }

    public required DateTime ExpiryDate { get; set; }

    public bool IsUsed { get; set; } = false;

    public required int UserId { get; set; }
}
