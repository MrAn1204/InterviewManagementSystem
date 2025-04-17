import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { InterviewModel, InterviewStatus } from '../../../../models/interview/interview.model';
import { INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../../services/user/user.service';
import { AuthService } from '../../../../services/auth/auth.service';
import { catchError, forkJoin, map, Observable, of, switchMap, tap } from 'rxjs';
import { UserInformation } from '../../../../models/auth/user-information.model';
import { User } from '../../../../models/user/user.model';

@Component({
  selector: 'app-interview-detail',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './interview-detail.component.html',
  styleUrl: './interview-detail.component.css'
})
export class InterviewDetailComponent implements OnInit {
  public interview!: InterviewModel;
  public id!: number;

  constructor(
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService,
    private readonly userService: UserService,
    public readonly authService: AuthService
  ) {
    this.userService = inject(UserService);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.id = Number(params.get('id'));
      this.interviewService.getById(this.id).subscribe((res) => this.interview = res);
    });
  }

  private getInterviewerInfo(): Observable<UserInformation[]> {
    if (this.authService.getUserRoles().some(role => ['ADMIN', 'RECRUITER', 'RECRUITER'].includes(role))) {
      return this.userService.getUsers({}).pipe(
        map((res) => res.items.filter(user => this.interview.interviewersId?.includes(user.id))
          .map(user => ({
            id: user.id.toString(),
            username: user.username,
            displayName: user.fullName,
            email: user.email,
            roles: user.roles
          }))
        )
      );
    } else {
      return this.authService.getUserInformation().pipe(
        map(info => [info!])
      )
    }
  }

  public sendReminder(): void {
    this.getInterviewerInfo().subscribe(interviewers => {
      interviewers.forEach(interviewer => {
        this.interviewService.sendReminder(interviewer.email, this.id, window.location.href)
          .pipe(
            tap(result => result
              ? this.toastr.success(`Email sent successfully to ${interviewer.username}`, 'Success')
              : this.toastr.info(`A reminder email has already been sent to ${interviewer.email}`, 'Info')
            ),
            catchError(() => {
              this.toastr.error(`Failed to send email to ${interviewer.email}`, 'Error');
              return of(null);
            }),
            tap(() => {
              this.interview.status = InterviewStatus[InterviewStatus.Invited];
              this.interviewService.update(this.id, this.interview).subscribe();
            })
          ).subscribe();
      });
    })
  }

  public checkInterviewed(): boolean {
    return this.interview.status !== InterviewStatus[InterviewStatus.Interviewed];
  }
}
