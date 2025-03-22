import { IAuthService } from './../../../services/auth/auth-service.interface';
import { Component, Inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  public form!: FormGroup;

  constructor(
    @Inject('IAuthService') private authService: IAuthService,
    private router: Router,
    private toastr: ToastrService // Inject ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  public createForm() {
    this.form = new FormGroup({
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255),
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
      ]),
    });
  }

  public onSubmit(): void {
    if (this.form.invalid) {
      this.toastr.warning('Please fill in all required fields!', 'Warning');
      return;
    }

    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        console.log('Login Response:', response);
        if (response) {
          this.toastr.success('Login successful!', 'Success');
          this.router.navigate(['/']);
        } else {
          this.toastr.error('Login failed!', 'Error');
        }
      },
      error: (err) => {
        console.error('Login Error:', err);
        this.toastr.error('Invalid username or password!', 'Error');
      }
    });
  }
}
