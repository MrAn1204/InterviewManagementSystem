import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private readonly _isCollapsed: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  private readonly _isMobile: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  public isCollapsed$: Observable<boolean> =
    this._isCollapsed.asObservable();
  public isMobile$: Observable<boolean> =
    this._isMobile.asObservable();

  get isCollapsed(): boolean {
    return this._isCollapsed.value;
  }
  get isMobile(): boolean {
    return this._isMobile.value;
  }

  constructor() {
  }

  toggleSidebar(): void {
    this._isCollapsed.next(!this._isCollapsed.value);
  }
  switchMobile(): void {
    this._isMobile.next(!this._isMobile.value);
  }
}
