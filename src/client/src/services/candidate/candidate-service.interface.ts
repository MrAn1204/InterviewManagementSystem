import { Observable } from "rxjs";
import { CandidateModel } from "../../models/candidate/candidate.model";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";
import { CandidateCreateModel } from "../../models/candidate/candidate-create.model";
import { CandidateUpdateModel } from "../../models/candidate/candidate-update.model";

export interface ICandidateService {
    getAll(): Observable<CandidateModel[]>;

    search(filter: any): Observable<PaginatedResult<CandidateModel>>;
  
    getById(id: number): Observable<CandidateModel>;
  
    create(data: CandidateCreateModel): Observable<boolean>;
  
    update(id: string, data: CandidateUpdateModel): Observable<boolean>;
  
    delete(id: string): Observable<boolean>;
}
