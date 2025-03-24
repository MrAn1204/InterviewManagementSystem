import { CommonModule } from '@angular/common';
import { Component, HostListener, Inject, OnInit, Renderer2 } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SidebarService } from '../../../../../services/sidebar/sidebar.service';
import { UserInformation } from '../../../../../models/auth/user-information.model';
import { IAuthService } from '../../../../../services/auth/auth-service.interface';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header',
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  public faBars: IconDefinition = faBars;
  public isShowProfileDropdown: boolean = false;
  public profileMenuElement!: HTMLElement;
  userInfo: UserInformation | null | undefined;

  constructor(
    @Inject('IAuthService') private authService: IAuthService,
    public sidebarService: SidebarService, private renderer: Renderer2) { }

  ngOnInit(): void {
    this.authService.getUserInformation().subscribe((data) => {
      this.userInfo = data;
    });
  }

  @HostListener('document:click', ['$event.target'])
  clickOutside(event: MouseEvent) {
    if (
      this.profileMenuElement &&
      !this.profileMenuElement.contains(event.target as Node)
    ) {
      this.isShowProfileDropdown = false;
    }
  }

  ngAfterViewInit() {
    const profileMenu = this.renderer.selectRootElement('.profile-menu', true);
    if (profileMenu) {
      this.profileMenuElement = profileMenu;
    }
  }

  toggleProfileDropdown(event: Event): void {
    event.stopPropagation();
    this.isShowProfileDropdown = !this.isShowProfileDropdown;
  }


}
