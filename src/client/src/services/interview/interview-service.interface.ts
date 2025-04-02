import { Observable } from "rxjs";
import { InterviewModel } from "../../models/interview/interview.model";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";

export interface IInterviewService {
    getAll(): Observable<InterviewModel[]>;

    search(filter: any): Observable<PaginatedResult<InterviewModel>>;

    getById(id: number): Observable<InterviewModel>;

    create(interview: any): Observable<boolean>;

    update(
        id: number,
        interview: any
    ): Observable<boolean>;

    delete(id: number): Observable<boolean>;
}
