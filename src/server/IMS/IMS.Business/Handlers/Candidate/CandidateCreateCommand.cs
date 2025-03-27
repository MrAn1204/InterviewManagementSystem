using System;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.Handlers.Candidate;

public class CandidateCreateCommand : BaseCreateCommand<bool>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public int Gender { get; set; }
    public IFormFile CV { get; set; }
    public string Note { get; set; }
    public int MyProperty { get; set; }
}
