import { Injectable } from '@angular/core';
import { IAuthService } from './auth-service.interface';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest } from '../../models/auth/login-request.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { UserInformation } from '../../models/auth/user-information.model';
import { HttpClient } from '@angular/common/http';
import { ForgotPasswordRequest } from '../../models/auth/forgot-password-request.model';
import { ResetPasswordRequest } from '../../models/auth/reset-password-request.model';

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
    // Check if the access token is present in local storage
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      this._isAuthenticated.next(true);
    }
    const userInformation = localStorage.getItem('userInformation');
    if (userInformation) {
      this._userInformation.next(JSON.parse(userInformation));
    }
  }
  getAccessToken(): string {
    return localStorage.getItem('accessToken') || '';
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
    // Remove the access token from local storage
    localStorage.removeItem('accessToken');
    // Remove the user information from local storage
    localStorage.removeItem('userInformation');
    // and set the isAuthenticated subject to false
    this._isAuthenticated.next(false);
    // Set the user information subject to null
    this._userInformation.next(null);
  }

  public login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(this.apiUrl + '/login', loginRequest)
      .pipe(
        tap((response: LoginResponse) => {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem(
            'userInformation',
            JSON.stringify(response.userInfo)
          );
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
