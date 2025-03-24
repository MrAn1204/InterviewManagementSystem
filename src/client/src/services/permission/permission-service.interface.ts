export interface IPermissionService {
  canActivate(): boolean;
  isUnauthenticated(): boolean;
  getAccessToken(): string;
  hasRole(roles: string[]): boolean;
}
