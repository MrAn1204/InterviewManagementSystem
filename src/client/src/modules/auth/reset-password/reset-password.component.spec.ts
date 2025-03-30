import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordComponent } from './reset-password.component';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { ToastrService } from 'ngx-toastr';
import { provideRouter, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from '../login/login.component';
import { of, throwError } from 'rxjs';
import { ResetPasswordRequest } from '../../../models/auth/reset-password-request.model';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;
  let mockAuthService: jasmine.SpyObj<IAuthService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;
  let router: Router;

  const validPasswordForm = {
    newPassword: 'NewPassword@123',
    confirmNewPassword: 'NewPassword@123',
  };

  const invalidPasswordForm = {
    newPassword: 'password',
    confirmNewPassword: 'password',
  };

  const missingMatchPasswordForm = {
    newPassword: 'NewPassword@123',
    confirmNewPassword: '',
  }

  const notMatchPasswordForm = {
    newPassword: 'NewPassword@123',
    confirmNewPassword: 'password',
  };

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('IAuthService', ['resetPassword']);
    mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'error', 'warning']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, ResetPasswordComponent],
      providers: [
        { provide: 'IAuthService', useValue: mockAuthService },
        { provide: ToastrService, useValue: mockToastr },
        provideRouter([
          { path: 'reset-password', component: ResetPasswordComponent },
          { path: 'login', component: LoginComponent }
        ])       
      ]
    })
    .compileComponents();

    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create form with empty values', () => {
    expect(component.resetPasswordForm.value).toEqual({
      newPassword: '',
      confirmNewPassword: ''
    });
  });

  it('should reset password when valid', () => {
    component.resetPasswordForm.setValue(validPasswordForm);
    expect(component.resetPasswordForm.valid).toBeTrue();

    const navigateSpy = spyOn(router, 'navigate');
    mockAuthService.resetPassword.and.returnValue(of(true));

    component.onSubmit();

    const request: ResetPasswordRequest = {
      token: '',
      ...validPasswordForm
    }

    expect(mockAuthService.resetPassword).toHaveBeenCalledWith(request);
    expect(mockToastr.success).toHaveBeenCalledWith('Password reset successful! Redirecting...', 'Success');
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('should show warning message when invalid', () => {
    component.resetPasswordForm.setValue(invalidPasswordForm);
    expect(component.resetPasswordForm.valid).toBeFalse();

    component.resetPasswordForm.controls['newPassword'].markAsTouched();
    fixture.detectChanges();
    
    const errorMessage = fixture.nativeElement.querySelector('#new-password-error');
    expect(errorMessage).toBeTruthy();
    expect(errorMessage.textContent).toContain(
      'Password must be at least 6 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.'
    );

    const resetPasswordButton = fixture.nativeElement.querySelector('#reset-password-button');
    expect(resetPasswordButton).toBeTruthy();
    expect(resetPasswordButton.disabled).toBeTrue();
  });

  it('should show warning message when match password is empty', () => {
    component.resetPasswordForm.setValue(missingMatchPasswordForm);
    expect(component.resetPasswordForm.valid).toBeFalse();

    component.resetPasswordForm.controls['confirmNewPassword'].markAsTouched();
    fixture.detectChanges();
    
    const errorMessage = fixture.nativeElement.querySelector('#confirm-password-error');
    expect(errorMessage).toBeTruthy();
    expect(errorMessage.textContent).toContain(
      'Passwords must match.'
    );

    const resetPasswordButton = fixture.nativeElement.querySelector('#reset-password-button');
    expect(resetPasswordButton).toBeTruthy();
    expect(resetPasswordButton.disabled).toBeTrue();
  });

  it('should show warning message when not match password', () => {
    component.resetPasswordForm.setValue(notMatchPasswordForm);

    mockAuthService.resetPassword.and.returnValue(throwError(() => new Error()));

    component.onSubmit();

    expect(mockAuthService.resetPassword).not.toHaveBeenCalled();
    expect(mockToastr.warning).toHaveBeenCalledWith('Passwords do not match!', 'Warning');
  });
});
