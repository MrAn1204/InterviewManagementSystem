import { Observable } from "rxjs";
import { InterviewModel } from "../../models/interview/interview.model";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";

export interface IInterviewService {
    getAll(): Observable<InterviewModel[]>;

    search(filter: any): Observable<PaginatedResult<InterviewModel>>;

    getById(id: number): Observable<InterviewModel>;

    create(interview: any): Observable<InterviewModel>;

    update(
        id: number,
        interview: InterviewModel
    ): Observable<InterviewModel>;

    delete(id: number): Observable<boolean>;
}
