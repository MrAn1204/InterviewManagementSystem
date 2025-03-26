import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyLoadingService } from '../services/busy-loading.service';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyLoadingService);
  busyService.busy();

  return next(req).pipe(
    delay(1000),
    finalize(() => {
      busyService.idle()
    })
  )
};
