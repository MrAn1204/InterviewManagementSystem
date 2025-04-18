import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { InterviewModel, InterviewStatus } from '../../../../models/interview/interview.model';
import { COMMON_SERVICE, INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../../services/user/user.service';
import { AuthService } from '../../../../services/auth/auth.service';
import { catchError, forkJoin, map, Observable, of, tap } from 'rxjs';
import { UserInformation } from '../../../../models/auth/user-information.model';
import { formatName, formatNameList, formatTime } from '../../../../helpers/format-schedule.helper';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';

@Component({
  selector: 'app-interview-detail',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './interview-detail.component.html',
  styleUrl: './interview-detail.component.css'
})
export class InterviewDetailComponent implements OnInit {
  public interview!: InterviewModel;
  public id!: number;

  private interviewers!: UserForInputModel[];
  private recruiter!: UserForInputModel;

  constructor(
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    @Inject(COMMON_SERVICE) private readonly commonService: ICommonService,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService,
    private readonly userService: UserService,
    public readonly authService: AuthService
  ) {
    this.userService = inject(UserService);
  }

  ngOnInit(): void {
    this.route.paramMap.pipe(
      tap((params) => {
        this.id = Number(params.get('id'));
      }),
      tap(() => {
        forkJoin({
          interview: this.interviewService.getById(this.id),
          interviewers: this.commonService.getUserForInputData(['INTERVIEWER']),
          recruiter: this.commonService.getUserForInputData(['RECRUITER'])
        }).pipe(
          tap((res) => {
            this.interview = res.interview;
            this.interviewers = res.interviewers.filter(i => this.interview.interviewersId?.includes(i.id));
            this.recruiter = res.recruiter.find(i => i.id === this.interview.recruiterId)!;
          })
        ).subscribe();
      })
    ).subscribe();
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

  public mapTime(time: string): string {
    return formatTime(time);
  }

  public mapInterviewerNames(): string[] {
    if (!this.interviewers) return [];

    return formatNameList(this.interviewers);
  }

  public mapRecruiterName(): string {
    if (!this.recruiter) return '';

    return formatName(this.recruiter);
  }
}
