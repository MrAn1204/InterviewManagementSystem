import { Injectable } from '@angular/core';
import { JobModel } from '../../models/job/job.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  private readonly url = 'http://localhost:5113/api/Job';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<JobModel[]> {
    return this.httpClient.get<JobModel[]>(this.url);
  }

  // search(filter: any): Observable<PaginatedResult<JobModel>> {
  //   return this.httpClient.post<PaginatedResult<JobModel>>(
  //     `${this.url}/search`,
  //     filter
  //   );
  // }

  // getById(id: number): Observable<JobModel> {
  //   return this.httpClient.get<JobModel>(`${this.url}/${id}`);
  // }


  // update(id: string, data: JobUpdateModel): Observable<boolean> {
  //   throw new Error('Method not implemented.');
  // }

  // delete(id: string): Observable<boolean> {
  //   throw new Error('Method not implemented.');
  // }
}
