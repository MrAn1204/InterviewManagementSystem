import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-inactive',
  imports: [],
  templateUrl: './user-inactive.component.html',
  styleUrl: './user-inactive.component.css'
})
export class UserInactiveComponent implements OnInit {
  userId!: number;
  isProcessing = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
  }

  confirmInactive(): void {
    this.isProcessing = true;
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
  }

  cancel(): void {
    this.router.navigate(['/admin/users', this.userId]);
  }
}
