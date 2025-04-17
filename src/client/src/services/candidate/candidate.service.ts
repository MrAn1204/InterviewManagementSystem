import { Injectable } from '@angular/core';
import { ICandidateService } from './candidate-service.interface';
import { Observable } from 'rxjs';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { PaginatedResult } from '../../models/candidate/paginated-result.model';
import { HttpClient } from '@angular/common/http';
import { CandidateCountByWeekModel } from '../../models/candidate/candidate-count-by-week.model';

@Injectable({
  providedIn: 'root',
})
export class CandidateService implements ICandidateService {
  private readonly url = 'http://localhost:5113/api/Candidate';

  constructor(private readonly httpClient: HttpClient) {}
  getCandidateCountByWeek(
    year: number,
    month: number
  ): Observable<CandidateCountByWeekModel[]> {
    return this.httpClient.get<CandidateCountByWeekModel[]>(
      `${this.url}/count-by-week?year=${year}&month=${month}`
    );
  }

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
      if (key !== 'cvAttachment' && key !== 'skills' && key !== 'gender') {
        formData.append(key, candidate[key as keyof typeof candidate]);
      }
    });
    if (candidate.gender != null) {
      formData.append('gender', candidate.gender);
    }
    candidate.skills.forEach((skillId: number) => {
      formData.append('skills', skillId.toString());
    });
    formData.append('cvAttachment', cvAttachment);
    return this.httpClient.post<boolean>(this.url, formData);
  }

  update(
    id: string,
    candidate: any,
    cvAttachment: File,
    oldFilePath: string
  ): Observable<boolean> {
    const formData = new FormData();
    formData.append('id', id);
    formData.append('oldFilePath', oldFilePath);
    Object.keys(candidate).forEach((key) => {
      if (
        key !== 'cvAttachment' &&
        key !== 'skills' &&
        key !== 'gender' &&
        key !== 'status'
      ) {
        formData.append(key, candidate[key as keyof typeof candidate]);
      }
    });
    if (candidate.gender != null) {
      formData.append('gender', candidate.gender);
    }
    candidate.skills.forEach((skillId: number) => {
      formData.append('skills', skillId.toString());
    });
    formData.append('cvAttachment', cvAttachment);
    return this.httpClient.post<boolean>(`${this.url}/update`, formData);
  }

  delete(id: number): Observable<boolean> {
    return this.httpClient.post<boolean>(`${this.url}/delete`, { id: id });
  }

  changeStatus(data: any): Observable<boolean> {
    return this.httpClient.post<boolean>(`${this.url}/status`, data);
  }
}
