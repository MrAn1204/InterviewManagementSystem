import { Observable } from "rxjs";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";
import { JobUpdateModel } from "../../models/job/job-update.model";
import { JobModel } from "../../models/job/job.model";

export interface IJobService {
    getAll(): Observable<JobModel[]>;

    search(filter: any): Observable<PaginatedResult<JobModel>>;

    getById(id: number): Observable<JobModel>;

    update(id: string, data: JobUpdateModel): Observable<boolean>;

    delete(id: string): Observable<boolean>;
}
