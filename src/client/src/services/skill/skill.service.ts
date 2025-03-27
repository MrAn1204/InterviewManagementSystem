import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SkillModel } from '../../models/skill/skill.modes';
import { HttpClient } from '@angular/common/http';
import { ISkillService } from './skill-service.interface';

@Injectable({
  providedIn: 'root',
})
export class SkillService implements ISkillService{
  private readonly url = 'http://localhost:5113/api/skill';
  constructor(private httpClient: HttpClient) {}
  public getAll(): Observable<SkillModel[]> {
    return this.httpClient.get<SkillModel[]>(this.url);
  }
}
