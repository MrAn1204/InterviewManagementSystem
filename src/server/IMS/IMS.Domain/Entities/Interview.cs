using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Interview : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Status { get; set; } = "Scheduled"; // or use ENUM
	public string? Location { get; set; }
	public string? MeetingID { get; set; }
	public string? Note { get; set; }

	public int CreatedBy { get; set; } // FK -> User
	public User? UserCreated { get; set; }

	// if 1:1 => candidateId, jobId
	public int? CandidateId { get; set; }

	public Candidate? Candidate { get; set; }

	public int? JobId { get; set; }
	public Job? Job { get; set; }

	// 1:N => Offer? (if not needed, remove)
	// but as stated, "Interview -> Offer = 1:N"? Actually we do "Offer -> interviewId"
	// so no direct navigation needed here unless we want it:
	public ICollection<Offer>? Offers { get; set; }
}