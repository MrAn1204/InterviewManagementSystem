import { Injectable } from '@angular/core';
import { IInterviewService } from './interview-service.interface';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../models/candidate/paginated-result.model';
import { InterviewModel } from '../../models/interview/interview.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InterviewService implements IInterviewService {
  private readonly url = 'http://localhost:5113/api/Interviews';

  constructor(private readonly httpClient: HttpClient) { }
  getAll(): Observable<InterviewModel[]> {
    return this.httpClient.get<InterviewModel[]>(this.url);
  }
  search(filter: any): Observable<PaginatedResult<InterviewModel>> {
    return this.httpClient.get<PaginatedResult<InterviewModel>>(`${this.url}/search`, { params: filter });
  }
  getById(id: number): Observable<InterviewModel> {
    return this.httpClient.get<InterviewModel>(`${this.url}/${id}`);
  }
  create(interview: any): Observable<InterviewModel> {
    return this.httpClient.post<InterviewModel>(this.url, interview);
  }
  update(id: number, interview: any): Observable<InterviewModel> {
    return this.httpClient.put<InterviewModel>(`${this.url}/${id}`, interview);
  }
  delete(id: number): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
}
