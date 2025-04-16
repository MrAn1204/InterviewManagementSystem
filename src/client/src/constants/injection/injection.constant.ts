import { InjectionToken } from '@angular/core';
import { IAuthService } from '../../services/auth/auth-service.interface';
import { IPermissionService } from '../../services/permission/permission-service.interface';
import { ICandidateService } from '../../services/candidate/candidate-service.interface';
import { ISkillService } from '../../services/skill/skill-service.interface';
import { ILevelService } from '../../services/level/level-sevice.interface';
import { ICommonService } from '../../services/data-for-input/common-service.interface';
import { IInterviewService } from '../../services/interview/interview-service.interface';
import { IBenefitService } from '../../services/benefit/benefit-service.interface';
import { IJobService } from '../../services/job/job-service.interface';
import { IOffService } from '../../services/offer/offer-service.interface';
import { IUserService } from '../../services/user/user-service.interface';

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
export const JOB_SERVICE = new InjectionToken<IJobService>('JOB_SERVICE');

export const OFFER_SERVICE = new InjectionToken<IOffService>('OFFER_SERVICE');

export const SKILL_SERVICE = new InjectionToken<ISkillService>('SKILL_SERVICE');
export const LEVEL_SERVICE = new InjectionToken<ILevelService>('LEVEL_SERVICE');
export const BENEFIT_SERVICE = new InjectionToken<IBenefitService>(
  'BENEFIT_SERVICE'
);
export const COMMON_SERVICE = new InjectionToken<ICommonService>(
  'COMMON_SERVICE'
);

export const USER_SERVICE = new InjectionToken<IUserService>('USER_SERVICE');

