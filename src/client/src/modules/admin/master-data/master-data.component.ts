import { IconDefinition } from '@fortawesome/angular-fontawesome';
import { faPlus, faRotate, faSearch } from '@fortawesome/free-solid-svg-icons';
import { FormGroup } from '@angular/forms';
import { OrderDirection, SearchModel } from '../../../models/search.model';
import { PaginatedResult } from '../../../models/paginated-result.model';
import { TableColumn } from '../../../core/models/table/table-column.model';

import { Component } from '@angular/core';

@Component({
  selector: 'app-master-data-list',
  templateUrl: './master-data.component.html',
  styleUrls: ['./master-data.component.css'],
})
export class MasterDataListComponent<T> {
  //#region Font Awesome icons
  public faPlus: IconDefinition = faPlus;
  public faSearch: IconDefinition = faSearch;
  public faRotate: IconDefinition = faRotate;

  //#endregion

  public isShowDetail: boolean = false;
  public filter: SearchModel = {
    keyword: '',
    status: '',
    pageNumber: 1,
    pageSize: 5,
    orderBy: '',
    orderDirection: OrderDirection.ASC,
  };
  public currentPage: number = 1;
  public currentPageSize: number = 5;
  public pageSizeOptions: number[] = [5, 10, 20, 50];

  public searchForm!: FormGroup;

  public data!: PaginatedResult<T>;

  public columns: TableColumn[] = [
    { name: 'Name', value: 'name' },
    { name: 'Description', value: 'description' },
  ];

  constructor() {}

  ngOnInit(): void {
    this.createForm();
    this.searchData();
  }

  protected createForm(): void {}

  protected searchData(): void {}

  public onSubmit(): void {
    // Gan gia tri tu form vao filter => Keyword
    Object.assign(this.filter, this.searchForm.value);
    this.searchData();
  }

  public onCloseDetail(): void {
    console.log('Event send from detail');
    this.isShowDetail = false;
    this.searchData();
  }

  public onPageChange(page: number): void {
    this.filter.pageNumber = page;
    if (page < 0 || page > this.data.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.searchData();
  }

  public onPageSizeChange(event: any): void {
    this.filter.pageSize = event.target.value;
    this.searchData();
  }
}
