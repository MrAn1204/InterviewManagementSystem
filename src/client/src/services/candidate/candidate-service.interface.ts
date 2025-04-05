import { Observable } from 'rxjs';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { PaginatedResult } from '../../models/candidate/paginated-result.model';
import { CandidateCreateModel } from '../../models/candidate/candidate-create.model';
import { CandidateUpdateModel } from '../../models/candidate/candidate-update.model';

export interface ICandidateService {
  getAll(): Observable<CandidateModel[]>;

  search(filter: any): Observable<PaginatedResult<CandidateModel>>;

  getById(id: number): Observable<CandidateModel>;

  create(candidate: any, cvAttachment: File): Observable<boolean>;

  update(
    id: string,
    candidate: any,
    cvAttachment: File,
    oldFilePath: string
  ): Observable<boolean>;

  delete(id: number): Observable<boolean>;
}
