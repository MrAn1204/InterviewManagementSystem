import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../../services/user/user.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgFor } from '@angular/common';
import { User } from '../../../../models/User';
import { HeaderService } from '../../../../services/header/header.service';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { PaginatedResult } from '../../../../models/paginated-result.model';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { InterviewTableComponent } from '../../interviews/interview-table/interview-table.component';
import { UserTableComponent } from "../user-table/user-table.component";

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, NgxPaginationModule, NgFor, RouterLink, UserTableComponent, ReactiveFormsModule, UserTableComponent],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends MasterDataListComponent<User> implements OnInit {
  headerService = inject(HeaderService);
  private userService = inject(UserService);
  private fb = inject(FormBuilder);
  private readonly router!: Router;
  roles: string[] = ['ADMIN', 'RECRUITER', 'INTERVIEWER', 'MANAGER'];

    public override columns: TableColumn[] = [
      { name: 'Full Name', value: 'fullName' },
      { name: 'Email', value: 'email' },
      { name: 'Address', value: 'address' },
      { name: 'Roles', value: 'roles' },
      { name: 'Status', value: 'isActive' }
    ];

  override ngOnInit(): void {
    // Đặt tiêu đề trang
    this.headerService.setTitle('User Management');
    // Gọi hàm khởi tạo của MasterDataListComponent: tạo form & load dữ liệu ban đầu
    super.ngOnInit();
  }

  // Khởi tạo reactive form dùng cho tìm kiếm
  protected override createForm(): void {
    this.searchForm = this.fb.group({
      keyword: [''],
      status: ['']
    });
  }

  // Gọi API tìm kiếm người dùng và gán dữ liệu vào this.data
  protected override searchData(): void {
    Object.assign(this.filter, this.searchForm.value);
    const searchParams = {
      search: this.filter.keyword,
      roles: this.filter.status ? [this.filter.status] : [],
      pageNumber: this.filter.pageNumber,
      pageSize: this.filter.pageSize,
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
