import { CandidateModel } from "../candidate/candidate.model";

// TODO: uncomment unavailable fields
export class InterviewModel {
    title!: string;
    status!: InterviewStatus;
    location!: string;
    schedule!: string;
    meetingId?: number;
    note?: string;
    createdBy!: string;
    candidate!: string;
    interviewers!: string[];
    recruiter!: string;
    job!: string;
    result?: InterviewResult;
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