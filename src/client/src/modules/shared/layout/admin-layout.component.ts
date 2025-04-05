import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { RouterOutlet } from '@angular/router';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { HeaderComponent } from '../common/nav/header/header.component';
import { SidebarComponent } from '../common/nav/sidebar/sidebar.component';

@Component({
  selector: 'app-admin-layout',
  imports: [CommonModule,HeaderComponent,SidebarComponent, RouterOutlet],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css'
})
export class AdminLayoutComponent {
  constructor(public sidebarService: SidebarService) { }
}
