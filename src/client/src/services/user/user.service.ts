import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import {  User } from '../../models/User';
import { environment } from '../../environments/environment.development';
import { PaginatedResult } from '../../models/paginated-result.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  baseUrl = environment.apiUrl;
  apiUrlList = `${this.baseUrl}users/list`;
  apiUrlCreate = `${this.baseUrl}users/create`;
  constructor(private http: HttpClient) { }

  getUsers(params: {
    search?: string;
    departmentId?: number;
    isActive?: boolean;
    roles?: string[];
    pageNumber: number;
    pageSize: number;
  }): Observable<PaginatedResult<User>> {
    let httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber.toString())
      .set('PageSize', params.pageSize.toString());

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


  createUser(user: Partial<User>): Observable<User> {
    return this.http.post<User>(`${this.apiUrlCreate}`, user).pipe(
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

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}users/${id}`);
  }

  updateUser(id: number, user: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}users/${id}`, user);
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
