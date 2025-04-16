import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { User } from '../../models/user/user.model';
import { environment } from '../../environments/environment.development';
import { PaginatedResult } from '../../models/paginated-result.model';
import { IUserService } from './user-service.interface';

@Injectable({ providedIn: 'root' })
export class UserService implements IUserService {
  baseUrl = environment.apiUrl;
  apiUrlList = `${this.baseUrl}users/list`;
  apiUrlCreate = `${this.baseUrl}users/create`;

  constructor(private readonly http: HttpClient) { }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}users`);
  }

  search(filter: any): Observable<PaginatedResult<User>> {
    return this.http.post<PaginatedResult<User>>(`${this.baseUrl}users/search`, filter);
  }

  getUsers(params: {
    search?: string;
    departmentId?: number;
    isActive?: boolean;
    roles?: string[];
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PaginatedResult<User>> {
    let httpParams = new HttpParams();

    if (params.pageNumber != null && params.pageSize != null) {
      httpParams = new HttpParams()
        .set('PageNumber', params.pageNumber.toString())
        .set('PageSize', params.pageSize.toString());
    }

    if (params.search) {
      httpParams = httpParams.set('Search', params.search);
    }

    if (params.departmentId !== null && params.departmentId !== undefined) {
      httpParams = httpParams.set('DepartmentId', params.departmentId.toString());
    }

    if (params.isActive !== null && params.isActive !== undefined) {
      httpParams = httpParams.set('IsActive', params.isActive.toString());
    }

    if (params.roles && params.roles.length > 0) {
      params.roles.forEach(r => {
        httpParams = httpParams.append('Roles', r);
      });
    }

    return this.http.get<PaginatedResult<User>>(this.apiUrlList, { params: httpParams });
  }

  create(user: Partial<User>): Observable<User> {
    return this.http.post<User>(this.apiUrlCreate, user).pipe(
      catchError((error) => {
        console.error('Error creating user:', error);
        return throwError(() => error);
      })
    );
  }

  checkUnique(username: string | null, email: string | null): Observable<{ usernameExists: boolean, emailExists: boolean }> {
    let params = new HttpParams();
    if (username) {
      params = params.set('username', username);
    }
    if (email) {
      params = params.set('email', email);
    }
    return this.http.get<{ usernameExists: boolean, emailExists: boolean }>(`${this.baseUrl}users/check-unique`, { params });
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}users/${id}`);
  }

  update(id: number, user: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}users/${id}`, user);
  }

  // Giả sử delete được triển khai, nếu cần
  delete(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}users/${id}`);
  }

  inactiveUser(id: number): Observable<any> {
    return this.http.put(`${this.baseUrl}users/${id}/inactive`, {})
      .pipe(catchError(err => throwError(() => err)));
  }

  activeUser(id: number): Observable<any> {
    return this.http.put(`${this.baseUrl}users/${id}/active`, {})
      .pipe(catchError(err => throwError(() => err)));
  }
}
