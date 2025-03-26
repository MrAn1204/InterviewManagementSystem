import { IAuthService } from './auth-service.interface';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest } from '../../models/auth/login-request.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { UserInformation } from '../../models/auth/user-information.model';
import { HttpClient } from '@angular/common/http';
import { ForgotPasswordRequest } from '../../models/auth/forgot-password-request.model';
import { ResetPasswordRequest } from '../../models/auth/reset-password-request.model';
import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  private apiUrl: string = 'http://localhost:5113/api/auth';

  private _isAuthenticated: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);

  private _isAuthenticated$: Observable<boolean> =
    this._isAuthenticated.asObservable();

  private _userInformation: BehaviorSubject<UserInformation | null> =
    new BehaviorSubject<UserInformation | null>(null);

  private _userInformation$: Observable<UserInformation | null> =
    this._userInformation.asObservable();

  constructor(private httpClient: HttpClient) {
    const lsToken = localStorage.getItem('accessToken');
    const ssToken = sessionStorage.getItem('accessToken');

    if (lsToken) {
      // -> rememberMe = true
      this._isAuthenticated.next(true);
      const userInformation = localStorage.getItem('userInformation');
      if (userInformation) {
        this._userInformation.next(JSON.parse(userInformation));
      }
    } else if (ssToken) {
      // -> rememberMe = false
      this._isAuthenticated.next(true);
      const userInformation = sessionStorage.getItem('userInformation');
      if (userInformation) {
        this._userInformation.next(JSON.parse(userInformation));
      }
    }


  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken') ?? sessionStorage.getItem('accessToken') ?? '';
  }

  getRefreshToken(): string {
    return localStorage.getItem('refreshToken') ?? sessionStorage.getItem('refreshToken') ?? '';
  }

  public isAuthenticated(): Observable<boolean> {
    return this._isAuthenticated$;
  }

  public getUserInformation(): Observable<UserInformation | null> {
    return this._userInformation$;
  }

  public getUserInformationFromAccessToken(): Observable<UserInformation | null> {
    // Using JWT to decode the access token and get the user information
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      const payload = JSON.parse(atob(accessToken.split('.')[1]));
      const userInformation: UserInformation = {
        id: payload.nameid,
        email: payload.email,
        displayName: payload.fullName,
        username: payload.unique_name,
        roles:
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ],
      };
      this._userInformation.next(userInformation);
    }
    return this._userInformation$;
  }

logout(): void {
  this.httpClient.post<boolean>(`${this.apiUrl}/logout`, { refreshToken: this.getRefreshToken() });
  
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('userInformation');
  sessionStorage.removeItem('accessToken');
  sessionStorage.removeItem('refreshToken');
  sessionStorage.removeItem('userInformation');
  this._isAuthenticated.next(false);
  this._userInformation.next(null);
}

public login(loginRequest: LoginRequest, rememberMe: boolean): Observable<LoginResponse> {
  return this.httpClient
    .post<LoginResponse>(`${this.apiUrl}/login`, loginRequest)
    .pipe(
      tap((response: LoginResponse) => {
        if (rememberMe) {
          // Lưu vào localStorage
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          localStorage.setItem('userInformation', JSON.stringify(response.userInfo));
        } else {
          // Lưu vào sessionStorage
          sessionStorage.setItem('accessToken', response.accessToken);
          sessionStorage.setItem('refreshToken', response.refreshToken);
          sessionStorage.setItem('userInformation', JSON.stringify(response.userInfo));
        }
        this._isAuthenticated.next(true);
        this._userInformation.next(response.userInfo);
      })
    );
}


  public forgotPassword(
    forgotPasswordRequest: ForgotPasswordRequest
  ): Observable<any> {
    console.log(forgotPasswordRequest);
    return this.httpClient.post(
      `${this.apiUrl}/forgot-password`,
      forgotPasswordRequest
    );
  }

  resetPassword(
    resetPasswordRequest: ResetPasswordRequest
  ): Observable<boolean> {
    return this.httpClient.post<boolean>(
      `${this.apiUrl}/reset-password`,
      resetPasswordRequest
    );
  }
}
