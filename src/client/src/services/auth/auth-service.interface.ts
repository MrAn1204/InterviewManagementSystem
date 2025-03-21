import { Observable } from "rxjs";
import { LoginRequest } from "../../models/auth/login-request.model";
import { LoginResponse } from "../../models/auth/login-response.model";
import { UserInformation } from "../../models/auth/user-information.model";

export interface IAuthService {
    login(loginRequest: LoginRequest): Observable<LoginResponse>;
    logout(): void;
    isAuthenticated(): Observable<boolean>;
    getUserInformation(): Observable<UserInformation | null>;
    getUserInformationFromAccessToken(): Observable<UserInformation | null>;
}
