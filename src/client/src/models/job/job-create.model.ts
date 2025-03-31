export class JobCreateModel {
    id!: number;
    title!: string;
    workingAddress!: string;
    salaryMin!: number;
    salaryMax!: number;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    status!: string;
    createdBy!: number;
}