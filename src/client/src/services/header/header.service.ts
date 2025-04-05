import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  private _title: BehaviorSubject<string> =
    new BehaviorSubject<string>('Homepage');
  public title$: Observable<string> =
    this._title.asObservable();
  constructor() { }
  setTitle(title: string): void {
    this._title.next(title);
  }
}
