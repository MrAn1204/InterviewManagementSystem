using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.Handlers;

public class CandidateUpdateCommand : IRequest<bool>
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [MinLength(1, ErrorMessage = "Full Name cannot be empty.")]
    [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [MinLength(1, ErrorMessage = "Email cannot be empty.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DOB { get; set; }

    [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string? Address { get; set; }

    [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [Range(-1, 1, ErrorMessage = "Gender invalid")]
    public int Gender { get; set; }

    public IFormFile? CvAttachment { get; set; }

    [MaxLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "Position is required.")]
    [MinLength(1, ErrorMessage = "Position cannot be empty.")]
    [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
    public required string Position { get; set; }

    [Required(ErrorMessage = "At least one skill is required.")]
    [MinLength(1, ErrorMessage = "At least one skill must be selected.")]
    public List<int> Skills { get; set; } = [];

    [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years.")]
    public int Experience { get; set; }

    [Required(ErrorMessage = "Recruiter ID is required.")]
    public int Recruiter { get; set; }

    [Required(ErrorMessage = "Highest Level is required.")]
    public int HighestLevel { get; set; }

    public string? OldFilePath { get; set; }
}
