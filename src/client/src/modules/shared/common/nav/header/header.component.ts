import { CommonModule } from '@angular/common';
import { Component, HostListener, Inject, OnInit, Renderer2 } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SidebarService } from '../../../../../services/sidebar/sidebar.service';
import { UserInformation } from '../../../../../models/auth/user-information.model';
import { IAuthService } from '../../../../../services/auth/auth-service.interface';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HeaderService } from '../../../../../services/header/header.service';

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
  public userInfo: UserInformation | null | undefined;


  constructor(
    @Inject('IAuthService') private readonly authService: IAuthService,
    public sidebarService: SidebarService,
    public headerService: HeaderService,
    private readonly renderer: Renderer2,
    private readonly router: Router,
    private readonly toastr: ToastrService
  ) { }

  public ngOnInit(): void {
    this.authService.getUserInformation().subscribe((data) => {
      this.userInfo = data;
    });
  }
  public onLogout(): void {
    // Gọi logout từ AuthService
    this.authService.logout();
    // Điều hướng về trang login (hoặc trang tuỳ ý)
    this.router.navigate(['/login']);
    this.toastr.warning('You were Logout');
  }
  @HostListener('document:click', ['$event.target'])
  public clickOutside(event: MouseEvent) {
    if (
      this.profileMenuElement &&
      !this.profileMenuElement.contains(event.target as Node)
    ) {
      this.isShowProfileDropdown = false;
    }
  }

  public ngAfterViewInit(): void {
    const profileMenu = this.renderer.selectRootElement('.profile-menu', true);
    if (profileMenu) {
      this.profileMenuElement = profileMenu;
    }
  }

  public toggleProfileDropdown(event: Event): void {
    event.stopPropagation();
    this.isShowProfileDropdown = !this.isShowProfileDropdown;
  }

}
