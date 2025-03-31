import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { AuthService } from '../services/auth/auth.service';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';
import { loadingInterceptor } from '../interceptors/loading.interceptor';
import { BENEFIT_SERVICE, CANDIDATE_SERVICE, DATA_FOR_INPUT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../constants/injection/injection.constant';
import { CandidateService } from '../services/candidate/candidate.service';
import { SkillService } from '../services/skill/skill.service';
import { LevelService } from '../services/level/level.service';
import { DataForInputService } from '../services/data-for-input/data-for-input.service';
import { BenefitService } from '../services/benefit/benefit.service';
import { JobService } from '../services/job/job.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    {
      provide: 'IAuthService',
      useClass: AuthService,
    },
    {
      provide: CANDIDATE_SERVICE,
      useClass: CandidateService,
    },
    {
      provide: JOB_SERVICE,
      useClass: JobService,
    },
    {
      provide: SKILL_SERVICE,
      useClass: SkillService,
    },
    {
      provide: LEVEL_SERVICE,
      useClass: LevelService,
    },
    {
      provide: BENEFIT_SERVICE,
      useClass: BenefitService,
    },
    {
      provide: DATA_FOR_INPUT_SERVICE,
      useClass: DataForInputService,
    },
    provideHttpClient(
      withFetch(),
      withInterceptors([loadingInterceptor])
    ),
    provideAnimations(),
    provideToastr(),
    importProvidersFrom(NgxSpinnerModule)
  ]
};
