import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent implements OnInit {
  public forgetPasswordForm!: FormGroup;

  constructor(
    @Inject('IAuthService') private authService: IAuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  private createForm(): void {
    this.forgetPasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  onSubmit() {
    if (this.forgetPasswordForm.invalid) {
      this.toastr.warning("Please enter a valid email address.", "Warning");
      return;
    }

    const requestData = { email: this.forgetPasswordForm.value.email };
    this.authService.forgotPassword(requestData).subscribe({
      next: () => {
        this.toastr.success("A password reset link has been sent to your email.", "Success");
        setTimeout(() => this.router.navigate(['/login']), 3000);
      },
      error: () => {
        this.toastr.error("Failed to send password reset email. Please try again.", "Error");
      }
    });
  }
}
