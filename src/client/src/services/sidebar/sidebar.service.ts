import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private _isCollapsed: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  public isCollapsed$: Observable<boolean> =
    this._isCollapsed.asObservable();

  get isCollapsed(): boolean {
    return this._isCollapsed.value;
  }

  constructor() {
  }

  toggleSidebar(): void {
    this._isCollapsed.next(!this._isCollapsed.value);
  }
}
