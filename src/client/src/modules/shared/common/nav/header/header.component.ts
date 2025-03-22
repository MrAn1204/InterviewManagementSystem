import { CommonModule } from '@angular/common';
import { Component, HostListener, Renderer2 } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SidebarService } from '../../../../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-header',
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  public faBars: IconDefinition = faBars;
  public isShowProfileDropdown: boolean = false;
  public profileMenuElement!: HTMLElement;

  constructor(public sidebarService: SidebarService, private renderer: Renderer2) { }

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
