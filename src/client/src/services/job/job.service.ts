import { Injectable } from '@angular/core';
import { JobModel } from '../../models/job/job.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { IJobService } from './job-service.interface';
import { BenefitModel } from '../../models/data-for-input/benefit.modes';
import { SkillModel } from '../../models/data-for-input/skill.modes';
import { LevelModel } from '../../models/data-for-input/level.model';
import { Observable } from 'rxjs';
import { JobImportResult } from '../../models/job/job-import-result.model';
import { JobCountByWeekModel } from '../../models/job/job-count-by-week.model';

@Injectable({
  providedIn: 'root',
})
export class JobService
  extends MasterDataService<JobModel>
  implements IJobService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'jobs');
  }
  getJobCountByWeek(
    year: number,
    month: number
  ): Observable<JobCountByWeekModel[]> {
    return this.httpClient.get<JobCountByWeekModel[]>(
      `${this.baseUrl}/count-by-week?year=${year}&month=${month}`
    );
  }

  importJobs(file: File, createdBy: number): Observable<JobImportResult> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    formData.append('createdBy', createdBy.toString());
    return this.httpClient.post<JobImportResult>(
      this.baseUrl + '/import',
      formData
    );
  }

  public getBenefitNames(benefits: BenefitModel[]): string {
    return (
      benefits?.map((level) => level.benefitName).join(', ') ||
      'No benefits specified'
    );
  }

  public getSkillNames(skills: SkillModel[]): string {
    return (
      skills?.map((skill) => skill.skillName).join(', ') ||
      'No skills specified'
    );
  }

  public getLevelNames(levels: LevelModel[]): string {
    return (
      levels?.map((level) => level.levelName).join(', ') ||
      'No levels specified'
    );
  }
}
