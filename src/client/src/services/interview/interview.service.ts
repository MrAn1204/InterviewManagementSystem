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
    throw new Error('Method not implemented.');
  }
  create(interview: any): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
  update(id: number, interview: any): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
  delete(id: number): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
}
