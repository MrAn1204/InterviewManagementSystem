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
import { UserTableComponent } from "../user-table/user-table.component";
import { Department } from '../../../../models/Department';
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
  private userService = inject(UserService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private departmentService = inject(DepartmentService);

  // Danh sách Role mà bạn muốn hiển thị (nếu cần sử dụng cho mục đích khác)
  roles: string[] = ['ADMIN', 'RECRUITER', 'INTERVIEWER', 'MANAGER'];
  // Danh sách Department (sẽ load từ API)
  departments: Department[] = [];

  // Cập nhật cột để hiển thị đầy đủ thông tin
  public override columns: TableColumn[] = [
    { name: 'Full Name', value: 'fullName' },
    { name: 'Email', value: 'email' },
    { name: 'Department', value: 'departmentName' },
    { name: 'Address', value: 'address' },
    { name: 'Roles', value: 'roles' },
    { name: 'Active', value: 'isActive' }
  ];

  override ngOnInit(): void {
    // Đặt tiêu đề trang
    this.headerService.setTitle('User Management');
    // Tải danh sách department để dùng cho filter
    this.loadDepartments();
    // Khởi tạo form tìm kiếm
    super.ngOnInit();
  }

  private loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (depts) => this.departments = depts,
      error: (err) => console.error('Error loading departments:', err)
    });
  }

  // Khởi tạo reactive form cho tìm kiếm
  protected override createForm(): void {
    this.searchForm = this.fb.group({
      keyword: [''],
      departmentId: [''],
      status: ['']
    });
  }

  // Hàm gọi API tìm kiếm và gán kết quả vào this.data
  protected override searchData(): void {
    // Gộp giá trị filter từ form vào filter hiện có
    Object.assign(this.filter, this.searchForm.value);

    const searchParams = {
      search: this.filter.keyword,
      departmentId: this.filter.departmentId ? Number(this.filter.departmentId) : undefined,
      isActive: this.filter.status === '' ? undefined : this.filter.status === 'true',
      pageNumber: this.filter.pageNumber,
      pageSize: this.filter.pageSize,
      roles: []  // Nếu không lọc theo role, giữ mảng rỗng
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
