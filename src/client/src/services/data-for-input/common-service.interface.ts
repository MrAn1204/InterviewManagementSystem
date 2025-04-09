import { Observable } from 'rxjs';
import { SkillModel } from '../../models/data-for-input/skill.modes';
import { LevelModel } from '../../models/data-for-input/level.model';
import { UserForInputModel } from '../../models/data-for-input/user-for-input.model';
import { CandidateStatusModel } from '../../models/candidate/candidate-status.model';

export interface ICommonService {
  getSkillData(): Observable<SkillModel[]>;
  getLevelData(): Observable<LevelModel[]>;
  getUserForInputData(roles: string[]): Observable<UserForInputModel[]>;
  getSelectableCandidateStatus(): Observable<CandidateStatusModel[]>;
  getAllCandidateStatus(): Observable<CandidateStatusModel[]>;
}
