import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IBenefitService } from './benefit-service.interface';
import { BenefitModel } from '../../models/data-for-input/benefit.modes';

@Injectable({
  providedIn: 'root',
})
export class BenefitService implements IBenefitService{
  private readonly url = 'http://localhost:5113/api/benefit';
  constructor(private httpClient: HttpClient) {}
  public getAll(): Observable<BenefitModel[]> {
    return this.httpClient.get<BenefitModel[]>(this.url);
  }
}
