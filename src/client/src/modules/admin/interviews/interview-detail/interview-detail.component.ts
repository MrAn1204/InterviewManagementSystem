import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { AUTH_SERVICE, INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ToastrService } from 'ngx-toastr';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { UserService } from '../../../../services/user/user.service';

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
    private readonly userService: UserService
  ) {
    this.userService = inject(UserService);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.id = Number(params.get('id'));
      this.interviewService.getById(this.id).subscribe((res) => this.interview = res);
    });
  }

  public sendReminder() {
    this.interview.interviewersId?.forEach(interviewerId => {
      this.userService.getUserById(interviewerId).subscribe((interviewer) => {
        this.interviewService.sendReminder(interviewer.email, this.id, window.location.href).subscribe({
          next: (result) => result
            ? this.toastr.success(`Email sent successfully to user ${interviewer.username}`, 'Success')
            : this.toastr.info(`A reminder email has already been sent to user ${interviewer.email}`, 'Info'),
          error: () => {
            this.toastr.error(`Failed to send email to user ${interviewer.email}`, 'Error');
          }
        });
      })
    })
  }
}
