using System;

namespace IMS.Business.ViewModels;

public class CandidateCountByWeekViewModel
{
    public int WeekNumber { get; set; }
    public int Year { get; set; }
    public int CandidateCount { get; set; }
}
