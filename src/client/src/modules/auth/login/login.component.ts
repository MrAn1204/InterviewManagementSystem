import { IAuthService } from './../../../services/auth/auth-service.interface';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { AUTH_SERVICE } from '../../../constants/injection/injection.constant';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public form!: FormGroup;

  constructor(
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  public createForm(): void {
    this.form = new FormGroup({
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255)
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20)
      ]),
      rememberMe: new FormControl(false)
    });
  }

  public onSubmit(): void {
    if (this.form.invalid) {
      this.toastr.warning('Please fill in all required fields!', 'Warning');
      return;
    }

    const loginRequest = {
      username: this.form.value.username,
      password: this.form.value.password
    };

    const rememberMe: boolean = this.form.value.rememberMe;

    // Gọi hàm login từ AuthService, hàm login sẽ tự lưu token và cập nhật _userInformation
    this.authService.login(loginRequest, rememberMe).subscribe({
      next: () => {
        this.toastr.success('Login successful!', 'Success');
        this.router.navigate(['/admin/dashboard']);
      },
      error: (err) => {
        const msg = err.message === 'Account inactive'
          ? 'Your account is inactive and cannot login'
          : 'Invalid username or password!';
        this.toastr.error(msg, 'Error');
      }
    });



  }
}
