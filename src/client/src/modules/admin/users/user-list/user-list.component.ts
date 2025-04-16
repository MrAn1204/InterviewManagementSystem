import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../../services/user/user.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgFor } from '@angular/common';
import { User } from '../../../../models/user/user.model';
import { HeaderService } from '../../../../services/header/header.service';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { PaginatedResult } from '../../../../models/paginated-result.model';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { UserTableComponent } from "../user-table/user-table.component";
import { Department } from '../../../../models/data-for-input/department.model';
import { DepartmentService } from '../../../../services/department/department.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, NgxPaginationModule, NgFor, RouterLink, UserTableComponent, ReactiveFormsModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends MasterDataListComponent<User> implements OnInit {
  headerService = inject(HeaderService);
  private readonly userService = inject(UserService);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly departmentService = inject(DepartmentService);

  roles: string[] = ['ADMIN', 'RECRUITER', 'INTERVIEWER', 'MANAGER'];
  departments: Department[] = [];

  public override columns: TableColumn[] = [
    { name: 'Full Name', value: 'fullName' },
    { name: 'Email', value: 'email' },
    { name: 'Department', value: 'departmentName' },
    { name: 'Address', value: 'address' },
    { name: 'Roles', value: 'roles' },
    { name: 'Status', value: 'isActive' }
  ];

  override ngOnInit(): void {
    this.headerService.setTitle('User');
    this.loadDepartments();
    super.ngOnInit();
  }

  private loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (depts) => this.departments = depts,
      error: (err) => console.error('Error loading departments:', err)
    });
  }

  protected override createForm(): void {
    this.searchForm = this.fb.group({
      keyword: [''],
      departmentId: [''],
      status: ['']
    });
  }

  protected override searchData(): void {
    Object.assign(this.filter, this.searchForm.value);

    const searchParams = {
      search: this.filter.keyword,
      departmentId: this.filter.departmentId ? Number(this.filter.departmentId) : undefined,
      isActive: this.filter.status === '' ? undefined : this.filter.status === 'true',
      pageNumber: this.filter.pageNumber,
      pageSize: this.filter.pageSize,
      roles: []
    };

    this.userService.getUsers(searchParams).subscribe({
      next: (response: PaginatedResult<User>) => {
        this.data = response;
        this.currentPage = this.filter.pageNumber;
        this.currentPageSize = this.filter.pageSize;
      },
      error: (err) => console.error('Error loading data:', err)
    });
  }


  public onSearch(): void {
    this.filter.pageNumber = 1;
    this.searchData();
  }
}
