import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LevelModel } from '../../models/data-for-input/level.model';
import { ILevelService } from './level-sevice.interface';

@Injectable({
  providedIn: 'root',
})
export class LevelService implements ILevelService{
  private readonly url = 'http://localhost:5113/api/level';
  constructor(private httpClient: HttpClient) {}
  public getAll(): Observable<LevelModel[]> {
    return this.httpClient.get<LevelModel[]>(this.url);
  }
}
