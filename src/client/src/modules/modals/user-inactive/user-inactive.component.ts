import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-user-inactive',
  templateUrl: './user-inactive.component.html',
  styleUrls: ['./user-inactive.component.css']
})
export class UserInactiveComponent implements OnInit {
  userId!: number;
  currentStatus!: boolean; // true: active, false: inactive
  isProcessing = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    // Đọc trạng thái hiện tại từ query param, nếu không có thì mặc định true
    this.currentStatus = this.route.snapshot.queryParamMap.get('currentStatus') === 'true';
  }

  confirmToggleStatus(): void {
    this.isProcessing = true;
    if (this.currentStatus) {
      // Nếu currentStatus = true (user đang active), gọi API inactivate
      this.userService.inactiveUser(this.userId).subscribe({
        next: () => {
          this.toastr.success('User has been inactivated', 'Success');
          this.router.navigate(['/admin/users']);
        },
        error: err => {
          this.toastr.error('Failed to inactivate user', 'Error');
          console.error(err);
          this.isProcessing = false;
        }
      });
    } else {
      // Nếu currentStatus = false (user đang inactive), gọi API activate
      this.userService.activeUser(this.userId).subscribe({
        next: () => {
          this.toastr.success('User has been activated', 'Success');
          this.router.navigate(['/admin/users']);
        },
        error: err => {
          this.toastr.error('Failed to activate user', 'Error');
          console.error(err);
          this.isProcessing = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate([`/admin/users/${this.userId}/detail`]);
  }
}
