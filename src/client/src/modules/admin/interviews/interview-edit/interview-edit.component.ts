import { CommonModule } from '@angular/common';
import { Component, HostListener, inject, Inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { AUTH_SERVICE, CANDIDATE_SERVICE, COMMON_SERVICE, INTERVIEW_SERVICE, JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { InterviewModel, InterviewResult, InterviewStatus } from '../../../../models/interview/interview.model';
import { JobModel } from '../../../../models/job/job.model';
import { IJobService } from '../../../../services/job/job-service.interface';
import { ToastrService } from 'ngx-toastr';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { timeRangeValidator } from '../../../../validators/time-range.validator';
import { UserService } from '../../../../services/user/user.service';
import { forkJoin, map, of, switchMap, tap } from 'rxjs';
import { CancelModalComponent } from "../../../modals/cancel-modal/cancel-modal.component";

@Component({
  selector: 'app-interview-edit',
  imports: [RouterLink, CommonModule, ReactiveFormsModule, FormsModule, CancelModalComponent],
  templateUrl: './interview-edit.component.html',
  styleUrl: './interview-edit.component.css'
})
export class InterviewEditComponent {
  public form!: FormGroup;
  public interviewerInput!: number[];
  public selectedInterviewers: string[] = [];
  public selectedInterviewersId: number[] = [];
  public dropdownVisible = false;
  public interview!: InterviewModel;
  public resultList: string[] = Object.keys(InterviewResult).filter(key => isNaN(Number(key)));
  public statusList: string[] = Object.keys(InterviewStatus).filter(key => isNaN(Number(key)));

  private interviewId!: number;

  public interviewerList!: UserForInputModel[];

  public jobList!: JobModel[];

  public recruiterList!: UserForInputModel[];

  public candidateList!: CandidateModel[];

  public isModalOpen = false;

  private roles!: string[];

  constructor(
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    @Inject(CANDIDATE_SERVICE) private readonly candidateService: ICandidateService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    @Inject(COMMON_SERVICE) private readonly commonService: ICommonService,
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService
  ) {
    inject(UserService);
  }

  ngOnInit(): void {
    this.loadInitialData();
    this.loadInterviewData();
  }

  private loadInitialData(): void {
    this.authService.getUserInformation().subscribe((res) => {
      console.log(res?.roles);

      this.roles = res?.roles ?? [];
    });

    this.candidateService.getAll().subscribe(res => this.candidateList = res);
    this.jobService.getAll().subscribe(res => this.jobList = res);

    this.commonService.getUserForInputData(['RECRUITER'])
      .subscribe(res => this.recruiterList = res);

    this.commonService.getUserForInputData(['INTERVIEWER'])
      .subscribe(res => this.interviewerList = res);
  }

  private loadInterviewData(): void {
    this.route.paramMap.pipe(
      map(params => Number(params.get('id'))),
      tap(id => this.interviewId = id),
      switchMap(id => this.interviewService.getById(id)),
      tap(res => {
        this.interview = res;
        this.selectedInterviewersId = [...res.interviewersId ?? []];
        this.createForm(res);
        this.form.patchValue({ interviewersId: this.selectedInterviewersId });
      }),
      switchMap(res => {
        if (!res.interviewersId?.length) return of([]);
        return forkJoin(res.interviewersId.map(id => this.userService.getById(id)));
      })
    ).subscribe(users => {
      this.selectedInterviewers = users.map(user => `${user.fullName} (${user.username})`);
    });
  }

  public createForm(interview: InterviewModel) {
    this.form = new FormGroup({
      title: new FormControl<string>(interview.title, [Validators.required, Validators.maxLength(100)]),
      candidateId: new FormControl<number>(interview.candidateId ?? 0, [Validators.required, Validators.min(1)]),
      interviewDate: new FormControl<string>(interview.interviewDate, [Validators.required]),
      startTime: new FormControl<string>(interview.startTime, [Validators.required]),
      endTime: new FormControl<string>(interview.endTime, [Validators.required]),
      jobId: new FormControl<number>(interview.jobId ?? 0, [Validators.required, Validators.min(1)]),
      interviewersId: new FormControl<number[]>(this.selectedInterviewersId, [Validators.required, Validators.minLength(1)]),
      location: new FormControl<string>(interview.location ?? '', []),
      recruiterId: new FormControl<number>(interview.recruiterId ?? 0, [Validators.required, Validators.min(1)]),
      meetingId: new FormControl<string>(interview.meetingId ?? '', []),
      note: new FormControl<string>(interview.note ?? '', [Validators.maxLength(500)]),
      result: new FormControl<string | null>(interview.result ?? null, []),
      status: new FormControl<string>(interview.status.toString(), []),
    }, { validators: timeRangeValidator });
  }

  public onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.toastr.error('Invalid form', 'Error');
      return;
    }

    const data: InterviewModel = this.form.value;
    data.status = InterviewStatus.Interviewed.toString();

    this.sendUpdateRequest(data);
  }

  public toggleModal(): void {
    this.isModalOpen = !this.isModalOpen;
  }

  public confirmCancel(): void {
    const data: InterviewModel = this.form.value;
    data.status = InterviewStatus.Cancelled.toString();

    this.sendUpdateRequest(data, 'This interview has been cancelled');
  }

  private sendUpdateRequest(
    data: InterviewModel,
    successMessage: string = 'Update success'
  ): void {
    this.interviewService.update(this.interviewId, data).subscribe({
      next: (data) => {
        if (data) {
          this.toastr.success(successMessage, 'Success')
          this.router.navigate(['/admin/interviews']);
        }
      },
      error: () => {
        this.toastr.error('Failed to update jobs', 'Error');
      }
    });
  }

  toggleDropdown(): void {
    this.dropdownVisible = !this.dropdownVisible;
  }

  public updateInterviewerSelected(interview: UserForInputModel): void {
    const id = interview.id;
    const name = `${interview.fullName} (${interview.userName})`;

    const idIndex = this.selectedInterviewersId.indexOf(id);
    const nameIndex = this.selectedInterviewers.indexOf(name);

    if (idIndex === -1) {
      this.selectedInterviewersId.push(id);
      this.selectedInterviewers.push(name);
    } else {
      this.selectedInterviewersId.splice(idIndex, 1);
      this.selectedInterviewers.splice(nameIndex, 1);
    }

    this.form.patchValue({
      interviewersId: this.selectedInterviewersId
    });
  }

  @HostListener('document:click', ['$event'])
  closeDropdownOnClickOutside(event: MouseEvent): void {
    const dropdownElement = document.getElementById('interviewerDropdown');
    const inputElement = document.getElementById('interviewers');

    // Check if the clicked element is outside the dropdown and the input element
    if (dropdownElement && inputElement && !dropdownElement.contains(event.target as Node) && !inputElement.contains(event.target as Node)) {
      this.dropdownVisible = false;
    }
  }

  public assignMe() {
    this.authService.getUserInformation().subscribe((res) => {
      if (res?.roles.includes('RECRUITER')) {
        this.form.patchValue({
          recruiterId: res?.id
        });
      } else {
        this.toastr.info('You do not have RECRUITER role', 'Info');
      }
    })
  }

  public checkEditable(): boolean {
    return this.roles.includes('ADMIN') || this.roles.includes('MANAGER') || this.roles.includes('RECRUITER');
  }

  public checkCancelable(): boolean {
    const isNewOrInvited = this.interview.status === InterviewStatus[InterviewStatus.New]
      || this.interview.status === InterviewStatus[InterviewStatus.Invited];
    return this.checkEditable() && isNewOrInvited;
  }

  public mapTime(time: string): string {
    const [hours, minutes] = time.split(':');
    return `${hours}:${minutes} ${Number(hours) >= 12 ? 'PM' : 'AM'}`;
  }

  public mapInterviewerNames(): string {
    const interviewers = this.interviewerList.filter(i => this.interview.interviewersId!.includes(i.id));
    return interviewers.map(interviewer => `${interviewer.fullName} (${interviewer.userName})`).join(', ');
  }

  public mapRecruiterNames(): string {
    const recruiter = this.recruiterList.filter(r => this.interview.recruiterId === r.id)[0];

    return `${recruiter.fullName} (${recruiter.userName})`;
  }
}
