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
import { CANDIDATE_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../constants/injection/injection.constant';
import { CandidateService } from '../services/candidate/candidate.service';
import { SkillService } from '../services/skill/skill.service';
import { LevelService } from '../services/level/level.service';

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
      provide: SKILL_SERVICE,
      useClass: SkillService,
    },
    {
      provide: LEVEL_SERVICE,
      useClass: LevelService,
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
