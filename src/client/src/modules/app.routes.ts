import { ForgetPasswordComponent } from './auth/forget-password/forget-password.component';
import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';

export const routes: Routes = [
    // { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'forget-password', component: ForgetPasswordComponent },
    { path: 'reset-password', component: ResetPasswordComponent },
];
