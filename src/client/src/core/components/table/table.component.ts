import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import {
  faEdit,
  faTrash,
  faPlus,
  faSearch,
  faRotate,
  faAngleLeft,
  faAngleDoubleLeft,
  faAngleRight,
  faAngleDoubleRight,
  faEye,
} from '@fortawesome/free-solid-svg-icons';
import { PaginatedResult } from '../../../models/paginated-result.model';
import { TableColumn } from '../../models/table/table-column.model';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css',
})
export class TableComponent {
  //#region Font Awesome icons
  public faEdit: IconDefinition = faEdit;
  public faTrash: IconDefinition = faTrash;
  public faPlus: IconDefinition = faPlus;
  public faSearch: IconDefinition = faSearch;
  public faRotate: IconDefinition = faRotate;
  public faAngleLeft: IconDefinition = faAngleLeft;
  public faAngleDoubleLeft: IconDefinition = faAngleDoubleLeft;
  public faAngleRight: IconDefinition = faAngleRight;
  public faAngleDoubleRight: IconDefinition = faAngleDoubleRight;
  public faEye: IconDefinition = faEye;

  //#endregion

  @Input() columns: TableColumn[] = [];
  @Input() public isShowNumber?: boolean = true;
  @Input() public currentPage: number = 1;
  @Input() public currentPageSize: number = 10;

  @Input() public data!: PaginatedResult<any>;

  @Input() public pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  @Output() onView = new EventEmitter<number>();

  @Output() public onEdit: EventEmitter<number> = new EventEmitter<number>();

  @Output() public onDelete: EventEmitter<number> = new EventEmitter<number>();

  @Output() public onPageSizeChange: EventEmitter<any> =
    new EventEmitter<any>();

  @Output() public onPageChange: EventEmitter<number> =
    new EventEmitter<number>();

  public generatePageItems(): number[] {
    if (!this.data) {
      return [];
    }

    const totalPage = this.data.totalPages;
    return Array.from({ length: totalPage }, (_, i) => i + 1);
  }

  public generatePageInfo(): string {
    if (this.data) {
      return `Page ${this.currentPageSize * (this.data.pageNumber - 1) + 1} -
      ${
        this.currentPageSize * this.data.pageNumber > this.data.totalCount
          ? this.data.totalCount
          : this.currentPageSize * this.data.pageNumber
      } of ${this.data.totalCount}`;
    }

    return '';
  }
}
