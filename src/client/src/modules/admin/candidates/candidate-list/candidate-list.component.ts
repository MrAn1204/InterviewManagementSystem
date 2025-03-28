import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';

@Component({
  selector: 'app-candidate-list',
  imports: [RouterLink],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css'
})
export class CandidateListComponent implements OnInit {

  constructor(private headerService: HeaderService){}

  ngOnInit(): void {
    this.headerService.setTitle('Candidate');
  }

}
