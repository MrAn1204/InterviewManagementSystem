// import { CanActivateFn } from '@angular/router';
// import { PERMISSION_SERVICE } from '../constants/injection/injection.constant';
// import { inject } from '@angular/core';

// export const authGuard: CanActivateFn = (route, state) => {
//   const permissionsService = inject(PERMISSION_SERVICE);
//   return permissionsService.canActivate();
// };

import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    // Kiểm tra token trong localStorage (Remember me) hoặc sessionStorage
    const lsToken = localStorage.getItem('accessToken');
    const ssToken = sessionStorage.getItem('accessToken');

    // Nếu có token ở 1 trong 2 nơi => người dùng đã đăng nhập
    if (lsToken || ssToken) {
      return true;
    } else {
      // Chưa đăng nhập => chuyển hướng về /login
      this.router.navigate(['/login']);
      return false;
    }
  }
}

