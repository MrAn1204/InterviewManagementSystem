import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, provideRouter } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { LoginResponse } from '../../../models/auth/login-response.model';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<IAuthService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;
  let router: Router;

  const emptyForm = {
    username: '',
    password: '',
    rememberMe: false
  };

  const validForm = {
    user: {
      username: 'testuser',
      password: 'password123',
    },
    rememberMe: true
  };

  const response: LoginResponse = {
    accessToken: 'token',
    refreshToken: 'refresh',
    expiresAt: new Date(),
    userInfo: {
      id: "1",
      username: 'testuser',
      displayName: 'Test User',
      email: 'test@example.com',
      roles: ['admin']
    }
  }

  const noPasswordForm = {
    username: 'testuser',
    password: '',
    rememberMe: false
  };

  const wrongPasswordForm = {
    user: {
      username: 'testuser',
      password: 'wrongpassword',
    },
    rememberMe: true
  };

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('IAuthService', ['login']);
    mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'error', 'warning']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, LoginComponent],
      providers: [
        { provide: 'IAuthService', useValue: mockAuthService },
        { provide: ToastrService, useValue: mockToastr },
        provideRouter([
          { path: 'login', component: LoginComponent },
          { path: 'admin', children: [] }
        ])       
      ]
    }).compileComponents();
    
    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create form with empty values', () => {
    expect(component.form.value).toEqual(emptyForm);
    expect(component.form.invalid).toBeTrue();
  });

  it('should be valid when username and password are provided', () => {
    component.form.setValue({
      ...validForm.user,
      rememberMe: validForm.rememberMe
    });
    expect(component.form.valid).toBeTrue();

    mockAuthService.login.and.returnValue(of(response));

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith(validForm.user, validForm.rememberMe);
  });

  it('should warn user with invalid form submission', () => {
    component.form.setValue(noPasswordForm);
    component.form.markAllAsTouched();
    fixture.detectChanges();
  
    expect(component.form.valid).toBeFalse();
  
    const errorElement = fixture.nativeElement.querySelector('#error-message');
    expect(errorElement).toBeTruthy();
    expect(errorElement.textContent).toContain('Please fix the errors before submitting');
  
    component.onSubmit();
    expect(mockAuthService.login).not.toHaveBeenCalled();
  });

  it('should login with valid form submission', () => {
    component.form.setValue({
      ...validForm.user,
      rememberMe: validForm.rememberMe
    });

    mockAuthService.login.and.returnValue(of(response));
    const navigateSpy = spyOn(router, 'navigate');

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith(validForm.user, validForm.rememberMe);
    expect(mockToastr.success).toHaveBeenCalledWith(
      'Login successful!',
      'Success'
    );
    expect(navigateSpy).toHaveBeenCalledWith(['/admin']);
  });

  it('should show error on login failure', () => {
    component.form.setValue({
      ...wrongPasswordForm.user,
      rememberMe: wrongPasswordForm.rememberMe
    });

    mockAuthService.login.and.returnValue(throwError(() => new Error('Login failed')));
    component.onSubmit();

    expect(mockToastr.error).toHaveBeenCalledWith(
      'Invalid username or password!',
      'Error'
    );
  });
});