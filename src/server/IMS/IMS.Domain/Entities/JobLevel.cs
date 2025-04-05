using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class JobLevel
{
	public int JobId { get; set; }
	public int LevelId { get; set; }

	public string? Note { get; set; }

	public Job? Job { get; set; }
	public Level? Level { get; set; }
}