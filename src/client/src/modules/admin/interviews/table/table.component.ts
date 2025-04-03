import { Component, Input } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TableColumn } from './table-column.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {
  public faEdit: IconDefinition = faEdit;
  public faTrash: IconDefinition = faTrash
  
  @Input() columns: TableColumn[] = []
  @Input() public isShowNumber?: boolean = true;
  @Input() public currentPage: number = 1;
  @Input() public currentPageSize: number = 10;

  @Input() public data!: PaginatedResult<any>;

  @Input() public pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  constructor (private readonly router: Router) { }

  public edit(id: string): void {
    this.router.navigate([`/admin/interviews/${id}/edit`])
  }

  public view(id: string): void {
    this.router.navigate([`/admin/interviews/${id}/detail`])
  }
}
