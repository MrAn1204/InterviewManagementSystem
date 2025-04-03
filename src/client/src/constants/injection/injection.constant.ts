import { InjectionToken } from '@angular/core';
import { IAuthService } from '../../services/auth/auth-service.interface';
import { IPermissionService } from '../../services/permission/permission-service.interface';
import { ICandidateService } from '../../services/candidate/candidate-service.interface';
import { ISkillService } from '../../services/skill/skill-service.interface';
import { ILevelService } from '../../services/level/level-sevice.interface';
import { IDataForInputService } from '../../services/data-for-input/data-for-input-service.interface';
import { IInterviewService } from '../../services/interview/interview-service.interface';
import { IBenefitService } from '../../services/benefit/benefit-service.interface';
import { IJobService } from '../../services/job/job-service.interface';
import { IOffService } from '../../services/offer/offer-service.interface';

export const AUTH_SERVICE = new InjectionToken<IAuthService>('AUTH_SERVICE');
export const PERMISSION_SERVICE = new InjectionToken<IPermissionService>(
  'PERMISSON_SERVICE'
);
export const CANDIDATE_SERVICE = new InjectionToken<ICandidateService>(
  'CANDIDATE_SERVICE'
);
export const INTERVIEW_SERVICE = new InjectionToken<IInterviewService>(
  'INTERVIEW_SERVICE'
);
export const JOB_SERVICE = new InjectionToken<IJobService>(
  'JOB_SERVICE'
);

export const OFFER_SERVICE = new InjectionToken<IOffService>(
  'OFFER_SERVICE'
);

export const SKILL_SERVICE = new InjectionToken<ISkillService>('SKILL_SERVICE');
export const LEVEL_SERVICE = new InjectionToken<ILevelService>('LEVEL_SERVICE');
export const BENEFIT_SERVICE = new InjectionToken<IBenefitService>('BENEFIT_SERVICE');
export const DATA_FOR_INPUT_SERVICE = new InjectionToken<IDataForInputService>(
  'DATA_FOR_INPUT_SERVICE'
);
