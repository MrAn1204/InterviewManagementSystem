import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';

@Component({
  selector: 'app-job-list',
  imports: [RouterLink],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent {
  constructor(private headerService: HeaderService) { }

  ngOnInit(): void {
    this.headerService.setTitle('Job');
  }

}
