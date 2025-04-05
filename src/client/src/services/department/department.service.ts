import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private apiUrl = `${environment.apiUrl}departments`;

  constructor(private http: HttpClient) { }

  getAllDepartments(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
