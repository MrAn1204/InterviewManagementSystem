export class OfferModel {
  id!: number;
  candidateId!: number;
  candidateName?: string;
  candidateEmail?: string;
  interviewId?: number;
  interviewTitle?: string;
  position!: string;
  contractType!: string;
  contractStart!: Date;
  contractEnd?: Date;
  status!: string;
  approvedBy!: number;
  approver!: string;
  approvedDate?: Date;
  salaryBasic!: number;
  note?: string;
  dueDate?: Date;
  departmentId?: number;
  departmentName?: string;
}