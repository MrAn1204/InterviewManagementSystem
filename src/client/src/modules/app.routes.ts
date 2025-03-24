import { ForgetPasswordComponent } from './auth/forget-password/forget-password.component';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { AdminLayoutComponent } from './shared/layout/admin-layout.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
  // Root -> nếu đã login => admin, chưa => login
  // Hoặc đơn giản: redirectTo '/login'
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    // canActivate: [AuthGuard], // guard (nếu có)
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
