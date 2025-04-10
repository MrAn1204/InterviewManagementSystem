// app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { ForgetPasswordComponent } from './auth/forget-password/forget-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { AdminLayoutComponent } from './shared/layout/admin-layout.component';
import { AuthGuard } from '../guards/auth.guard';

// Import các component admin
import { UserListComponent } from './admin/users/user-list/user-list.component';
import { UserCreateComponent } from './admin/users/user-create/user-create.component';
import { UserEditComponent } from './admin/users/user-edit/user-edit.component';
import { UserDetailsComponent } from './admin/users/user-details/user-details.component';
import { CandidateListComponent } from './admin/candidates/candidate-list/candidate-list.component';
import { CandidateCreateComponent } from './admin/candidates/candidate-create/candidate-create.component';
import { CandidateEditComponent } from './admin/candidates/candidate-edit/candidate-edit.component';
import { CandidateDetailComponent } from './admin/candidates/candidate-detail/candidate-detail.component';
import { JobListComponent } from './admin/jobs/job-list/job-list.component';
import { JobCreateComponent } from './admin/jobs/job-create/job-create.component';
import { JobEditComponent } from './admin/jobs/job-edit/job-edit.component';
import { JobDetailComponent } from './admin/jobs/job-detail/job-detail.component';
import { InterviewListComponent } from './admin/interviews/interview-list/interview-list.component';
import { InterviewCreateComponent } from './admin/interviews/interview-create/interview-create.component';
import { InterviewEditComponent } from './admin/interviews/interview-edit/interview-edit.component';
import { InterviewDetailComponent } from './admin/interviews/interview-detail/interview-detail.component';
import { OfferListComponent } from './admin/offers/offer-list/offer-list.component';
import { OfferCreateComponent } from './admin/offers/offer-create/offer-create.component';
import { OfferEditComponent } from './admin/offers/offer-edit/offer-edit.component';
import { OfferDetailComponent } from './admin/offers/offer-detail/offer-detail.component';
import { SimpleLayoutComponent } from './shared/common/simple-layout/simple-layout.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { UserInactiveComponent } from './modals/user-inactive/user-inactive.component';
import { NotFoundComponent } from '../errors/not-found/not-found.component';

export const routes: Routes = [
  // Redirect root '' về '/login'
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path : 'home', component: AdminLayoutComponent},
  // Các route không cần layout admin: login, forget-password, reset-password
  {
    path: '',
    component: SimpleLayoutComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'forget-password', component: ForgetPasswordComponent },
      { path: 'reset-password', component: ResetPasswordComponent },
    ]
  },


  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        children: [
          { path: '', component: DashboardComponent },
        ]
      },
      {
        path: 'users',
        children: [
          { path: '', component: UserListComponent },
          { path: 'create', component: UserCreateComponent },
          { path: ':id/edit', component: UserEditComponent },
          { path: ':id/detail', component: UserDetailsComponent },
          { path: ':id/toggle-status', component: UserInactiveComponent },
        ]
      },
      {
        path: 'candidates',
        children: [
          { path: '', component: CandidateListComponent },
          { path: 'create', component: CandidateCreateComponent },
          { path: ':id/edit', component: CandidateEditComponent },
          { path: ':id/detail', component: CandidateDetailComponent }
        ]
      },
      {
        path: 'jobs',
        children: [
          { path: '', component: JobListComponent },
          { path: 'create', component: JobCreateComponent},
          { path: ':id/edit', component: JobEditComponent},
          { path: ':id/detail', component: JobDetailComponent}
        ]
      },
      {
        path: 'interviews',
        children: [
          { path: '', component: InterviewListComponent },
          { path: 'create', component: InterviewCreateComponent },
          { path: ':id/edit', component: InterviewEditComponent },
          { path: ':id/detail', component: InterviewDetailComponent }
        ]
      },
      {
        path: 'offers',
        children: [
          { path: '', component: OfferListComponent },
          { path: 'create', component: OfferCreateComponent },
          { path: ':id/edit', component: OfferCreateComponent },
          // { path: ':id/edit', component: OfferEditComponent },
          { path: ':id/detail', component: OfferDetailComponent }
        ]
      }
    ]
  },

  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
