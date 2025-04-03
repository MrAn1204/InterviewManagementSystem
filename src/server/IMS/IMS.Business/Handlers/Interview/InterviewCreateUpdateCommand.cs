using System;
using IMS.Business.ViewModels;
using IMS.Domain;

namespace IMS.Business.Handlers;

public class InterviewCreateUpdateCommand : BaseCreateUpdateCommand<InterviewViewModel>
{
    public required string Title { get; set; }

    public required int CandidateId { get; set; }

    public required DateOnly InterviewDate { get; set; }

    public required TimeOnly StartTime { get; set; }

    public required TimeOnly EndTime { get; set; }

    public string? Note { get; set; }

    public required int JobId { get; set; }

    public required ICollection<int> InterviewersId { get; set; }

    public string? Location { get; set; }

    public int RecruiterId { get; set; }

    public string? MeetingId { get; set; }

    public InterviewStatus Status { get; set; } = InterviewStatus.New;

    public InterviewResult? Result { get; set; }

    public int CreatedBy { get; set; }
}
