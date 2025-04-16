// admin-layout.component.ts
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { HeaderComponent } from '../common/nav/header/header.component';
import { SidebarComponent } from '../common/nav/sidebar/sidebar.component';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { filter } from 'rxjs';
import { BreadcrumbService } from '../../../services/breadcrumb/breadcrumb.service';
import { HeaderService } from '../../../services/header/header.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, HeaderComponent, 
    SidebarComponent, RouterOutlet, BreadcrumbComponent],
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent implements OnInit {
  constructor(
    public sidebarService: SidebarService,
    private readonly router: Router,
    private readonly breadcrumbService: BreadcrumbService,
    private readonly headerService: HeaderService
  ) { }
  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      setTimeout(() => this.breadcrumbService.refresh(), 0);
      setTimeout(() => this.headerService.refresh(), 0);    
    });

    setTimeout(() => this.breadcrumbService.refresh(), 0);
    setTimeout(() => this.headerService.refresh(), 0);
  }

  public isDashboardRoute(): boolean {
    let isDashboard = false;
    if (this.router.url === '/admin/dashboard') {
      isDashboard = true;
    }
    return isDashboard;
  }
}
