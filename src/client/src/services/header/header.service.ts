import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  private readonly _title: BehaviorSubject<string> =
    new BehaviorSubject<string>('');
  public title$: Observable<string> =
    this._title.asObservable();
  constructor(
    private readonly route: ActivatedRoute,
  ) { }

  public setTitle(title: string): void {
    this._title.next(title);
  }

  public refresh(): void {
    const currentRoute = this.route.root;
    let route = currentRoute;
    while (route.firstChild) {
      route = route.firstChild;
    }
    const title = route.snapshot.data['title'];
    if (title) {
      this.setTitle(title);
    }
  }
}
