import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { PERMISSION_SERVICE } from '../constants/injection/injection.constant';

export const roleGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PERMISSION_SERVICE);
  const requiredRoles = route.data['roles'] || [];
  return permissionService.hasRole(requiredRoles);
};
