import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { CommonModule } from '@angular/common';
import { ResetPasswordRequest } from '../../../models/auth/reset-password-request.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  public resetPasswordForm!: FormGroup;
  public token: string = '';
  public showPassword = false;
  public showConfirmPassword = false;

  constructor(
    @Inject('IAuthService') private readonly authService: IAuthService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService // Inject ToastrService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'] ?? '';
    });

    this.createForm();
  }

  private createForm(): void {
    this.resetPasswordForm = new FormGroup({
      newPassword: new FormControl('', [Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)]),
      confirmNewPassword: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    if (this.resetPasswordForm.invalid) {
      this.toastr.error("Please fill in all required fields.", "Validation Error");
      return;
    }

    const { newPassword, confirmNewPassword } = this.resetPasswordForm.value;

    if (newPassword !== confirmNewPassword) {
      this.toastr.warning("Passwords do not match!", "Warning");
      return;
    }

    const requestData: ResetPasswordRequest = {
      token: encodeURIComponent(this.token),
      newPassword: newPassword,
      confirmNewPassword: confirmNewPassword
    };

    this.authService.resetPassword(requestData).subscribe({
      next: () => {
        this.toastr.success("Password reset successful! Redirecting...", "Success");
        this.router.navigate(['/login']);
      },
      error: () => {
        this.toastr.error("An error occurred while resetting the password.", "Error");
      }
    });
  }


  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}
