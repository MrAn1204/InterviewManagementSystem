import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, provideRouter } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { LoginResponse } from '../../../models/auth/login-response.model';
import { By } from '@angular/platform-browser';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<IAuthService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;
  let router: Router;

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
    expect(component.form.value).toEqual({
      username: '',
      password: '',
      rememberMe: false
    });
    expect(component.form.invalid).toBeTrue();
  });

  it('should be valid when username and password are provided', () => {
    component.form.setValue({
      username: 'testuser',
      password: 'password123',
      rememberMe: true
    });
    expect(component.form.valid).toBeTrue();
  });

  it('should warn user with invalid form submission', () => {
    component.form.setValue({
      username: 'testuser',
      password: '',
      rememberMe: false
    });
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
    const loginResponse: LoginResponse = {
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
    };

    component.form.setValue({
      username: 'testuser',
      password: 'password123',
      rememberMe: true
    });

    mockAuthService.login.and.returnValue(of(loginResponse));
    const navigateSpy = spyOn(router, 'navigate');

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith(
      { username: 'testuser', password: 'password123' },
      true
    );
    expect(mockToastr.success).toHaveBeenCalledWith(
      'Login successful!',
      'Success'
    );
    expect(navigateSpy).toHaveBeenCalledWith(['/admin']);
  });

  it('should show error on login failure', () => {
    component.form.setValue({
      username: 'testuser',
      password: 'wrongpass',
      rememberMe: false
    });

    mockAuthService.login.and.returnValue(throwError(() => new Error('Login failed')));
    component.onSubmit();

    expect(mockToastr.error).toHaveBeenCalledWith(
      'Invalid username or password!',
      'Error'
    );
  });
});