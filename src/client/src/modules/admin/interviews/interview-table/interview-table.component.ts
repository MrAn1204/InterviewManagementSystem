import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Router } from '@angular/router';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule],
  templateUrl: './interview-table.component.html',
  styleUrl: './interview-table.component.css'
})
export class InterviewTableComponent extends TableComponent {
  constructor (private readonly router: Router) {
    super();
  }

  public edit(id: string): void {
    this.router.navigate([`/admin/interviews/${id}/edit`])
  }

  public view(id: string): void {
    this.router.navigate([`/admin/interviews/${id}/detail`])
  }
}
