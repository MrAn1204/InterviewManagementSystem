import { ResetPasswordRequest } from './../../models/auth/reset-password-request.model';
import { Observable } from "rxjs";
import { LoginRequest } from "../../models/auth/login-request.model";
import { LoginResponse } from "../../models/auth/login-response.model";
import { UserInformation } from "../../models/auth/user-information.model";
import { ForgotPasswordRequest } from "../../models/auth/forgot-password-request.model";

export interface IAuthService {
    login(loginRequest: LoginRequest, remember : boolean): Observable<LoginResponse>;
    logout(): void;
    isAuthenticated(): Observable<boolean>;
    getUserInformation(): Observable<UserInformation | null>;
    getUserInformationFromAccessToken(): Observable<UserInformation | null>;
    forgotPassword(forgotPasswordRequest: ForgotPasswordRequest): Observable<void>;
    resetPassword(resetPasswordRequest: ResetPasswordRequest): Observable<boolean>;
    getAccessToken(): string;
    getUserRoles() : string[];
    hasRole(allowedRoles: string[]): boolean;
}
