using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class JobBenefit
{
	public int JobId { get; set; }
	public int BenefitId { get; set; }

	public Job? Job { get; set; }
	public Benefit? Benefit { get; set; }
}