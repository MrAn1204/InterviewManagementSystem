import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 401:
          router.navigate(['/access-denied']);
          break;
        case 404:
          router.navigate(['/not-found']);
          break;
        case 500: {
          const navigationExtras: NavigationExtras = {state: {error: error.error}};
          router.navigateByUrl('/server-error', navigationExtras);
          break;
        }
        default: {
            const errorMessage = error.error?.message ?? 'An unexpected error occurred.';
            toastr.error(errorMessage, `Error ${error.status}`);
            break;
        }
      }
      return throwError(() => error);
    })
  );
};
