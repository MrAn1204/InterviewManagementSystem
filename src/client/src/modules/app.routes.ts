import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { ForgetPasswordComponent } from './auth/forget-password/forget-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';

import { AdminLayoutComponent } from './shared/layout/admin-layout.component';
import { AuthGuard } from '../guards/auth.guard';

export const routes: Routes = [
  // 1) Khi vào root '', ta chuyển hướng sang '/login'
  { path: '', redirectTo: '/login', pathMatch: 'full' },

  // 2) Đường dẫn /login
  { path: 'login', component: LoginComponent },

  // 3) Đường dẫn /forget-password
  { path: 'forget-password', component: ForgetPasswordComponent },

  // 4) Đường dẫn /reset-password
  { path: 'reset-password', component: ResetPasswordComponent },

  // 5) Đường dẫn /admin => chỉ vào được nếu AuthGuard = true
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard], // Bảo vệ /admin
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },

  // 6) Bắt các đường dẫn không khớp => chuyển hướng /login (hoặc 404)
  { path: '**', redirectTo: '/login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
