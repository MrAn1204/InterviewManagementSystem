import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Data, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { Breadcrumb } from './Breadcrumb';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  private readonly _breadcrumbs = new BehaviorSubject<Breadcrumb[]>([]);
  public readonly breadcrumbs$ = this._breadcrumbs.asObservable();

  constructor(private readonly router: Router) {
    // Lắng nghe sự kiện NavigationEnd
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.createBreadcrumbs();
    });

    // Khởi tạo breadcrumbs ngay lập tức khi service được tạo
    // Điều này sẽ đảm bảo breadcrumbs được tạo ngay cả khi tải lại trang
    setTimeout(() => {
      this.createBreadcrumbs();
    }, 0);
  }

  private createBreadcrumbs(): void {
    if (this.router.url.includes('/admin/dashboard')) {
      this._breadcrumbs.next([]);
      return;
    }

    const root = this.router.routerState.snapshot.root;
    const breadcrumbs: Breadcrumb[] = [];
    
    breadcrumbs.push({
      label: 'Homepage',
      url: '/admin/dashboard'
    });
    
    this.addBreadcrumb(root, [], breadcrumbs);
    
    this._breadcrumbs.next(breadcrumbs);
  }

  private addBreadcrumb(
    route: ActivatedRouteSnapshot,
    parentUrl: string[],
    breadcrumbs: Breadcrumb[]
  ) {
    if (route) {
      const routeUrl = parentUrl.concat(route.url.map(url => url.path));
      const url = '/' + routeUrl.join('/');
      
      if (route.routeConfig?.path) {
        const label = this.getLabelForRoute(route);
        
        if (label) {
          breadcrumbs.push({
            label: label,
            url: url
          });
        }
      }
      
      if (route.firstChild) {
        this.addBreadcrumb(route.firstChild, routeUrl, breadcrumbs);
      }
    }
  }

  private getLabelForRoute(route: ActivatedRouteSnapshot): string {
    const path = route.routeConfig?.path;
    
    if (!path || path === '' || path === '**' || path === 'admin') {
      return '';
    }
    
    if (path.includes(':id/detail')) {
      return `Detail`;
    }
    
    if (path.includes(':id/edit')) {
      return `Edit`;
    }
    
    if (path === 'create') {
      return 'Create';
    }
    
    switch (path) {
      case 'dashboard': return '';
      case 'users': return 'Users';
      case 'candidates': return 'Candidates';
      case 'jobs': return 'Jobs';
      case 'interviews': return 'Interviews';
      case 'offers': return 'Offers';
      default:
        return path.split(/[-_]/)
          .map(word => word.charAt(0).toUpperCase() + word.slice(1))
          .join(' ');
    }
  }

  public refresh(): void {
    this.createBreadcrumbs();
  }
}