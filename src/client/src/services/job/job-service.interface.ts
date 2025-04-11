import { Observable } from 'rxjs';
import { BenefitModel } from '../../models/data-for-input/benefit.modes';
import { LevelModel } from '../../models/data-for-input/level.model';
import { SkillModel } from '../../models/data-for-input/skill.modes';
import { JobModel } from '../../models/job/job.model';
import { IMasterDataService } from '../master-data/master-data-service.interface';
import { JobImportResult } from '../../models/job/job-import-result.model';
import { JobCountByWeekModel } from '../../models/job/job-count-by-week.model';

export interface IJobService extends IMasterDataService<JobModel> {
  getBenefitNames(benefits: BenefitModel[]): string;

  getSkillNames(skills: SkillModel[]): string;

  getLevelNames(levels: LevelModel[]): string;

  importJobs(file: File, createdBy: number): Observable<JobImportResult>;
  
  getJobCountByWeek(
    year: number,
    month: number
  ): Observable<JobCountByWeekModel[]>;
}
