import { Injectable } from '@angular/core';
import { ICandidateService } from './candidate-service.interface';
import { Observable } from 'rxjs';
import { CandidateCreateModel } from '../../models/candidate/candidate-create.model';
import { CandidateUpdateModel } from '../../models/candidate/candidate-update.model';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { PaginatedResult } from '../../models/candidate/paginated-result.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CandidateService implements ICandidateService {
  private readonly url = 'http://localhost:5176/api/category';
  constructor(private httpClient: HttpClient) {}
  getAll(): Observable<CandidateModel[]> {
    throw new Error('Method not implemented.');
  }
  search(filter: any): Observable<PaginatedResult<CandidateModel>> {
    throw new Error('Method not implemented.');
  }
  getById(id: number): Observable<CandidateModel> {
    throw new Error('Method not implemented.');
  }
  create(data: CandidateCreateModel): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url, data);
  }
  update(id: string, data: CandidateUpdateModel): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
  delete(id: string): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
}
