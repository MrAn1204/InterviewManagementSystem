import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';

import { ForgetPasswordComponent } from './forget-password.component';
import { ReactiveFormsModule } from '@angular/forms';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { ToastrService } from 'ngx-toastr';
import { provideRouter, Router } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { of, throwError } from 'rxjs';

describe('ForgetPasswordComponent', () => {
  let component: ForgetPasswordComponent;
  let fixture: ComponentFixture<ForgetPasswordComponent>;
  let mockAuthService: jasmine.SpyObj<IAuthService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;
  let router: Router;


  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('IAuthService', ['forgotPassword']);
    mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'error', 'warning']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, ForgetPasswordComponent],
      providers: [
        { provide: 'IAuthService', useValue: mockAuthService },
        { provide: ToastrService, useValue: mockToastr },
        provideRouter([
          { path: 'forgot-password', component: ForgetPasswordComponent },
          { path: 'login', component: LoginComponent }
        ])       
      ]
    })
    .compileComponents();

    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(ForgetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create form with empty values', () => {
    expect(component.forgetPasswordForm.value).toEqual({
      email: ''
    });
  });

  it('should be send email when valid', fakeAsync(() => {
    component.forgetPasswordForm.setValue({
      email: 'user@domain.com'
    });
    expect(component.forgetPasswordForm.valid).toBeTrue();

    const navigateSpy = spyOn(router, 'navigate');
    mockAuthService.forgotPassword.and.returnValue(of(undefined));

    component.onSubmit();
    tick(3000);

    expect(mockAuthService.forgotPassword).toHaveBeenCalledWith({ email: 'user@domain.com' });
    expect(mockToastr.success).toHaveBeenCalledWith('A password reset link has been sent to your email.', 'Success');
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  }));

  it('should not send request with invalid email', () => {
    component.forgetPasswordForm.setValue({
      email: 'invalid-email'
    });
    expect(component.forgetPasswordForm.valid).toBeFalse();

    component.onSubmit();

    expect(mockAuthService.forgotPassword).not.toHaveBeenCalledWith({ email: 'invalid-email' });
  });

  it('should warn user with invalid email', fakeAsync(() => {
    component.forgetPasswordForm.setValue({
      email: 'unregistered@user.com'
    });
    expect(component.forgetPasswordForm.valid).toBeTrue();

    mockAuthService.forgotPassword.and.returnValue(throwError(() => new Error()));
    const navigateSpy = spyOn(router, 'navigate');

    component.onSubmit();
    tick(3000);

    expect(mockAuthService.forgotPassword).toHaveBeenCalledWith({ email: 'unregistered@user.com' });
    expect(mockToastr.error).toHaveBeenCalledWith('Failed to send password reset email. Please try again.', 'Error');
    expect(navigateSpy).not.toHaveBeenCalledWith(['/login']);
  }));
});
