import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PERMISSION_SERVICE } from '../constants/injection/injection.constant';

export const roleGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PERMISSION_SERVICE);
  const router = inject(Router);
  const requiredRoles: string[] = route.data['roles'] || [];

  if (permissionService.hasRole(requiredRoles)) {
    return true;
  } else {
    return router.parseUrl('/access-denied');
  }
};
