namespace IMS.Domain;

public enum CandidateStatus
{
    Open = 1,
    Banned = 2,
    WaitingForInterview = 3,
    CancelledInterview = 4,
    PassedInterview = 5,
    FailedInterview = 6,
    WaitingForApproval = 7,
    ApprovedOffer = 8,
    RejectedOffer = 9,
    WaitingForResponse = 10,
    AcceptedOffer = 11,
    DeclinedOffer = 12,
    CancelledOffer = 13
}
