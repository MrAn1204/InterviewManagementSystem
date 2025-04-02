export class InterviewModel {
    title!: string;
    interviewDate!: string;
    startTime!: string;
    endTime!: string;
    note?: string;
    location?: string;
    meetingId?: number;
    status!: InterviewStatus;
    result!: InterviewResult;
    candidateId?: number;
    candidateName?: string;
    interviewersId?: number[];
    interviewersName?: string[];
    recruiterId?: number;
    recruiterName?: string;
    jobId?: number;
    jobName?: string;
}

export enum InterviewStatus {
    New = 0,
    Invited = 1,
    Interviewed = 2,
    Cancelled = 3,
}

export enum InterviewResult {
    Failed = 0,
    Passed = 1,
}