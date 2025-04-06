import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../models/candidate/paginated-result.model';
import { OfferModel } from '../../models/offer/offer.model';
import { IOffService } from './offer-service.interface';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OfferService implements IOffService {
  private readonly url = 'http://localhost:5113/api/Offer';

  constructor(private httpClient: HttpClient) { }
  getAll(): Observable<OfferModel[]> {
    return this.httpClient.get<OfferModel[]>(this.url);
  }
  
  search(filter: any): Observable<OfferModel[]> {
      return this.httpClient.post<OfferModel[]>(
        `${this.url}/search`,
        filter
      );
  }

  getById(id: number): Observable<OfferModel> {
    return this.httpClient.get<OfferModel>(`${this.url}/${id}`);
  }

  create(data: any): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url, data);
  }
  
  update(id: number, data: any): Observable<boolean> {
    return this.httpClient.put<boolean>(`${this.url}/${id}`, data);
  }

  changeStatus(data: any): Observable<boolean> {
    return this.httpClient.post<boolean>(`${this.url}/status`, data);
  }

  exportOffers(data: any) {
    return this.httpClient.post<Blob>(
      `${this.url}/export`,
      data,
      { responseType: 'blob' as 'json' }
    );
  }
}
