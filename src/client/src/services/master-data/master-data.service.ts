import { Injectable } from '@angular/core';
import { IMasterDataService } from './master-data-service.interface';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../models/paginated-result.model';

@Injectable({
  providedIn: 'root',
})
export class MasterDataService<T> implements IMasterDataService<T> {
  protected readonly baseUrl = 'http://localhost:5113/api/';

  constructor(protected httpClient: HttpClient, protected apiUrl: String) {
    this.baseUrl += apiUrl;
  }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.baseUrl);
  }

  search(filter: any): Observable<PaginatedResult<T>> {
    return this.httpClient.post<PaginatedResult<T>>(
      this.baseUrl + '/search',
      filter
    );
  }

  getById(id: number): Observable<T> {
    return this.httpClient.get<T>(`${this.baseUrl}/${id}`);
  }

  create(data: T): Observable<T> {
    return this.httpClient.post<T>(this.baseUrl, data);
  }

  update(id: number, data: T): Observable<T> {
    return this.httpClient.put<T>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: number): Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.baseUrl}/${id}`);
  }
}
