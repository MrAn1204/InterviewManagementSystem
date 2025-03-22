import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { AdminLayoutComponent } from './shared/layout/admin-layout.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    {
        path: 'admin',
        component: AdminLayoutComponent,
        loadChildren: () =>
            import('./admin/admin.module').then((m) => m.AdminModule),
    },
];
