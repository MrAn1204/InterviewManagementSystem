import { IAuthService } from './../../../services/auth/auth-service.interface';
import { Component, Inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public form!: FormGroup;

  constructor(
    @Inject('IAuthService') private authService: IAuthService,
    private router: Router
  ) {
    this.authService.isAuthenticated().subscribe((res) => {
      if (res) {
        this.router.navigate(['/']);
      }
    });
  }

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
    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        console.log('Login Response:', response);
        if (response) {
          this.router.navigate(['/']);
        }
      },
      error: (err) => console.error('Login Error:', err)
    });
  }
}
