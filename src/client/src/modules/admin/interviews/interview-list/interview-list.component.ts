import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';

@Component({
  selector: 'app-interview-list',
  imports: [RouterLink],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.css'
})
export class InterviewListComponent {
    constructor(private headerService: HeaderService){}
  
    ngOnInit(): void {
      this.headerService.setTitle('Interview');
    }
  
}
