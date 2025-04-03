import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { User } from '../../../../models/User';
import { UserService } from '../../../../services/user/user.service';
import { DatePipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-user-details',
  imports: [RouterLink, DatePipe, NgIf],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css'
})
export class UserDetailsComponent implements OnInit {
  userDetail!: User;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.loadUserDetail(+userId);
    } else {
      this.router.navigate(['/admin/users']);
    }
  }

  private loadUserDetail(userId: number): void {
    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        this.userDetail = user;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading user detail:', err);
        this.router.navigate(['/admin/users']);
      }
    });
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }

  onEdit(): void {
    this.router.navigate([`/admin/users/edit/${this.userDetail.id}`]);
  }

}

