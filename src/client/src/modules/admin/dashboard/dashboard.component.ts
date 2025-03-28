import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../../../services/header/header.service';
import { SidebarService } from '../../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  constructor(public sidebarService: SidebarService,
    public headerService: HeaderService
  ) { }
  ngOnInit(): void {
    this.headerService.setTitle('Homepage');
  }
}
