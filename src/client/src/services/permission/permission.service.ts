import { Inject, Injectable } from '@angular/core';
import { IPermissionService } from './permission-service.interface';
import { Router } from '@angular/router';
import { AUTH_SERVICE } from '../../constants/injection/injection.constant';
import { IAuthService } from '../auth/auth-service.interface';

@Injectable({
  providedIn: 'root',
})
export class PermissionService implements IPermissionService {
  private roles: string[] = [];
  constructor(
    private readonly router: Router,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    }
    this.router.navigate(['/auth/login']);
    return false;
  }

  isUnauthenticated(): boolean {
    this.authService.isAuthenticated().subscribe((res) => {
      if (res) {
        this.router.navigate(['/']);
        return false;
      }
      return true;
    });
    return true;
  }

  getAccessToken(): string {
    return this.authService.getAccessToken();
  }

  hasRole(roles: string[]): boolean {
    this.authService.getUserInformation().subscribe((res) => {
      return roles.some(role=>res?.roles.includes(role));
    });
    return false;
  }

  setRoles(roles: string[]) {
    this.roles = roles;
  }
  clearRoles() {
    this.roles = [];
  }
}
