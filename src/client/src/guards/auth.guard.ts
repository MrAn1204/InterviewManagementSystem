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
  constructor(public router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Kiểm tra token
    const lsToken = localStorage.getItem('accessToken');
    const ssToken = sessionStorage.getItem('accessToken');

    if (lsToken || ssToken) {
      // Đã đăng nhập => cho vào
      return true;
    } else {
      // Chưa đăng nhập => chuyển hướng /login
      this.router.navigate(['/login']);
      return false;
    }
  }
}

