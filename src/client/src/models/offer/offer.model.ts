export class OfferModel {
  id!: number;
  candidateId!: number;
  interviewId!: number;
  position!: string;
  contracType!: string;
  contractStart?: string | null;
  contractEnd?: string | null;
  status!: string;
  approvedBy!: number;
  approvedDate?: string | null;
  salaryBasic?: string | null;
  note?: string | null;
  dueDate?: string | null;
  createdDate?: string | null;
  updatedDate?: string | null;
  departmentId!: number;
}