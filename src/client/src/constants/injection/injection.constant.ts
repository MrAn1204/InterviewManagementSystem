import { InjectionToken } from '@angular/core';
import { IAuthService } from '../../services/auth/auth-service.interface';
import { IPermissionService } from '../../services/permission/permission-service.interface';
import { ICandidateService } from '../../services/candidate/candidate-service.interface';
import { ISkillService } from '../../services/skill/skill-service.interface';
import { ILevelService } from '../../services/level/level-sevice.interface';

export const AUTH_SERVICE = new InjectionToken<IAuthService>('AUTH_SERVICE');
export const PERMISSION_SERVICE = new InjectionToken<IPermissionService>(
  'PERMISSON_SERVICE'
);
export const CANDIDATE_SERVICE = new InjectionToken<ICandidateService>(
  'CANDIDATE_SERVICE'
);

export const SKILL_SERVICE = new InjectionToken<ISkillService>('SKILL_SERVICE');
export const LEVEL_SERVICE = new InjectionToken<ILevelService>('LEVEL_SERVICE');
