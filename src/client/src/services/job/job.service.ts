import { Injectable } from '@angular/core';
import { JobModel } from '../../models/job/job.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { IJobService } from './job-service.interface';
import { BenefitModel } from '../../models/data-for-input/benefit.modes';
import { SkillModel } from '../../models/data-for-input/skill.modes';
import { LevelModel } from '../../models/data-for-input/level.model';

@Injectable({
  providedIn: 'root'
})
export class JobService extends MasterDataService<JobModel> implements IJobService{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Job');
  }
  
  public getBenefitNames(benefits: BenefitModel[]): string {
    return benefits?.map(level => level.benefitName).join(', ') || 'No benefits specified';
  }
  
  public getSkillNames(skills: SkillModel[]): string {
    return skills?.map(skill => skill.skillName).join(', ') || 'No skills specified';
  }

  public getLevelNames(levels: LevelModel[]): string {
    return levels?.map(level => level.levelName).join(', ') || 'No levels specified';
  }
}
