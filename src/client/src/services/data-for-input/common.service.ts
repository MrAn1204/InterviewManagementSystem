import { Injectable } from '@angular/core';
import { ICommonService } from './common-service.interface';
import { Observable } from 'rxjs';
import { LevelModel } from '../../models/data-for-input/level.model';
import { SkillModel } from '../../models/data-for-input/skill.modes';
import { UserForInputModel } from '../../models/data-for-input/user-for-input.model';
import { HttpClient } from '@angular/common/http';
import { CandidateStatusModel } from '../../models/candidate/candidate-status.model';

@Injectable({
  providedIn: 'root',
})
export class CommonService implements ICommonService {
  private readonly url = 'http://localhost:5113/api/Common';

  constructor(private readonly httpClient: HttpClient) {}

  public getSelectableCandidateStatus(): Observable<CandidateStatusModel[]> {
    return this.httpClient.get<CandidateStatusModel[]>(`${this.url}/getSelectableCandidateStatuses`);
  }

  public getAllCandidateStatus(): Observable<CandidateStatusModel[]> {
    return this.httpClient.get<CandidateStatusModel[]>(`${this.url}/getAllCandidateStatuses`);
  }

  public getSkillData(): Observable<SkillModel[]> {
    return this.httpClient.get<SkillModel[]>(`${this.url}/Skills`);
  }

  public getLevelData(): Observable<LevelModel[]> {
    return this.httpClient.get<LevelModel[]>(`${this.url}/Levels`);
  }

  public getUserForInputData(roles: string[]): Observable<UserForInputModel[]> {
    return this.httpClient.post<UserForInputModel[]>(
      `${this.url}/UsersByRole`,
      { roles }
    );
  }
}
