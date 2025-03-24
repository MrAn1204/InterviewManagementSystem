import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { PERMISSION_SERVICE } from '../constants/injection/injection.constant';

export const anonymousGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PERMISSION_SERVICE);
  return permissionService.isUnauthenticated();
};
