import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BreadcrumbService } from '../../../services/breadcrumb/breadcrumb.service';
import { Breadcrumb } from '../../../services/breadcrumb/Breadcrumb';
import { faAngleRight, faHome, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-breadcrumb',
  imports: [RouterLink, CommonModule,FontAwesomeModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css'
})

export class BreadcrumbComponent implements OnInit {
  public breadcrumbs: Breadcrumb[] = [];
  public faHome: IconDefinition = faHome;
  public faAngleRight: IconDefinition = faAngleRight;

  constructor(
    private readonly breadcrumbService: BreadcrumbService,
    private readonly router: Router
  ) {}
  
  public ngOnInit() {
    this.breadcrumbService.breadcrumbs$.subscribe(breadcrumbs => {
      this.breadcrumbs = breadcrumbs;
    });
  }
}
