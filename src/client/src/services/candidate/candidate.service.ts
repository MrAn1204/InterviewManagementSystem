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
  private readonly url = 'http://localhost:5113/api/Candidate';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<CandidateModel[]> {
    return this.httpClient.get<CandidateModel[]>(this.url);
  }

  search(filter: any): Observable<PaginatedResult<CandidateModel>> {
    return this.httpClient.post<PaginatedResult<CandidateModel>>(
      `${this.url}/search`,
      filter
    );
  }

  getById(id: number): Observable<CandidateModel> {
    return this.httpClient.get<CandidateModel>(`${this.url}/${id}`);
  }

  create(candidate: any, cvAttachment: File): Observable<boolean> {
    const formData = new FormData();
    Object.keys(candidate).forEach((key) => {
      if (key !== 'cvAttachment' && key !== 'skills') {
        formData.append(key, candidate[key as keyof typeof candidate]);
      }
    });
    candidate.skills.forEach((skillId: number) => {
      formData.append('Skills', skillId.toString());
    });
    formData.append('cvAttachment', cvAttachment);
    return this.httpClient.post<boolean>(this.url, formData);
  }

  update(id: string, data: CandidateUpdateModel): Observable<boolean> {
    throw new Error('Method not implemented.');
  }

  delete(id: string): Observable<boolean> {
    throw new Error('Method not implemented.');
  }
}
