import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../../services/user/user.service';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgFor } from '@angular/common';
import { PaginatedResult, User } from '../../../../models/User';
import { HeaderService } from '../../../../services/header/header.service';

@Component({
  selector: 'app-user-list',
  imports: [ FormsModule,NgxPaginationModule, NgFor, RouterLink],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit {
  headerService = inject(HeaderService);
  users: User[] = [];
  roles: string[] = ['ADMIN', 'RECRUITER', 'INTERVIEWER', 'MANAGER'];
  searchQuery: string = '';
  selectedRole: string = '';
  page: number = 1;
  pageSize: number = 3;
  totalCount: number = 0;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
    this.headerService.setTitle('User Management')
  }

  loadUsers(): void {
    this.userService.getUsers({
      search: this.searchQuery,
      roles: this.selectedRole ? [this.selectedRole] : [],
      pageNumber: this.page,
      pageSize: this.pageSize
    }).subscribe({
      next: (response: PaginatedResult<User>) => {
        this.users = response.items;
        this.totalCount = response.totalCount;
      },
      error: (err) => console.error('Lỗi tải dữ liệu:', err)
    });
  }

  onSearch(): void {
    this.page = 1;
    this.loadUsers();
  }

  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadUsers();
    }
  }

  nextPage(): void {
    if (this.page * this.pageSize < this.totalCount) {
      this.page++;
      this.loadUsers();
    }
  }
}
