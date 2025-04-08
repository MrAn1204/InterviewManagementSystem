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
    roles?: string[];
    pageNumber: number;
    pageSize: number;
  }): Observable<PaginatedResult<User>> {
    let httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber.toString())
      .set('PageSize', params.pageSize.toString());

    if (params.search) httpParams = httpParams.set('Search', params.search);
    if (params.roles?.length) params.roles.forEach(r => httpParams = httpParams.append('Roles', r));

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
}
