import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  IconDefinition, faGear, faUser,
  faHome, faBriefcase, faCalendarAlt, faUsers,
  faFileAlt,
  faAngleDoubleLeft,
  faAngleDoubleRight
} from '@fortawesome/free-solid-svg-icons';
import { SidebarService } from '../../../../../services/sidebar/sidebar.service';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../../services/auth/auth.service';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, FontAwesomeModule, RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  public faHome: IconDefinition = faHome;
  public faGear: IconDefinition = faGear;
  public faBriefcase: IconDefinition = faBriefcase;
  public faFileAlt: IconDefinition = faFileAlt;
  public faUser: IconDefinition = faUser;
  public faUsers: IconDefinition = faUsers;
  public faCalendarAlt: IconDefinition = faCalendarAlt;
  public faAngleDoubleLeft: IconDefinition = faAngleDoubleLeft;
  public faAngleDoubleRight: IconDefinition = faAngleDoubleRight;

  constructor(public sidebarService: SidebarService, public authService: AuthService) { }

  @HostListener('window:resize', ['$event'])
  public onResize(event: any) {
    if (window.innerWidth < 768 && !this.sidebarService.isMobile) {
      this.switchMobile();
      if (!this.sidebarService.isCollapsed) {
        this.toggleSidebar();
      }
    }
    else if (window.innerWidth >= 768 && this.sidebarService.isMobile) {
      this.switchMobile()
      if (this.sidebarService.isCollapsed) {
        this.toggleSidebar();
      }
    }
  }

  public toggleSidebar(): void {
    this.sidebarService.toggleSidebar();
  }

  public switchMobile(): void {
    this.sidebarService.switchMobile();
  }
}
