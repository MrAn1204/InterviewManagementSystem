import { Component, Input } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TableColumn } from './table-column.model';
import { Router } from '@angular/router';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
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
