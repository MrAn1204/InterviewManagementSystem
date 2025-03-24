import { ForgetPasswordComponent } from './auth/forget-password/forget-password.component';
import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { AdminLayoutComponent } from './shared/layout/admin-layout.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // thêm dòng này
  { path: 'login', component: LoginComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
];
