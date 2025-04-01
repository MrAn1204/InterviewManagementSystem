using System;

namespace IMS.Business.ViewModels;

public class BenefitViewModel
{
    public int Id { get; set; }
    public string BenefitName { get; set; } = null!;
    public string? Description { get; set; }
}
