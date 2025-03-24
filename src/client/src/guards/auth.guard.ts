import { CanActivateFn } from '@angular/router';
import { PERMISSION_SERVICE } from '../constants/injection/injection.constant';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const permissionsService = inject(PERMISSION_SERVICE);
  return permissionsService.canActivate();
};
