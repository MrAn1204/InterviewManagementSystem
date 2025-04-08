import { CommonModule } from '@angular/common';
import { Component, HostListener, inject, Inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { AUTH_SERVICE, CANDIDATE_SERVICE, INTERVIEW_SERVICE, JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { InterviewModel, InterviewResult, InterviewStatus } from '../../../../models/interview/interview.model';
import { JobModel } from '../../../../models/job/job.model';
import { IJobService } from '../../../../services/job/job-service.interface';
import { ToastrService } from 'ngx-toastr';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { UserService } from '../../../../services/user/user.service';
import { User } from '../../../../models/User';

@Component({
  selector: 'app-interview-edit',
  imports: [RouterLink, CommonModule, ReactiveFormsModule, FormsModule],
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

  public interviewerList!: User[];

  public jobList!: JobModel[];

  public recruiterList!: User[];

  public candidateList!: CandidateModel[];

  constructor(
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    @Inject(CANDIDATE_SERVICE) private readonly candidateService: ICandidateService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService
  ) {
    this.userService = inject(UserService);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.interviewId = Number(params.get('id'));
      this.interviewService.getById(this.interviewId).subscribe((res) => {
        this.interview = res;

        // delays execution until after Angular's change detection cycle finishes
        setTimeout(() => {
          this.selectedInterviewers = [... this.interview.interviewersName ?? []];
        }, 0);

        this.selectedInterviewersId = [... this.interview.interviewersId ?? []];

        this.createForm(res);

        console.log(this.interview.interviewersId);

        this.form.patchValue({
          interviewersId: this.selectedInterviewersId
        });
      });
    });

    this.candidateService.getAll().subscribe((res) => this.candidateList = res);
    this.jobService.getAll().subscribe((res) => this.jobList = res);

        
    this.userService.getUsers({ pageNumber: 1, pageSize: 100 })
      .subscribe((res) => this.recruiterList = res.items.filter(user => user.roles.includes('RECRUITER')));
    this.userService.getUsers({ pageNumber: 1, pageSize: 100 })
      .subscribe((res) => this.interviewerList = res.items.filter(user => user.roles.includes('INTERVIEWER')));
  }

  public createForm(interview: InterviewModel) {
    this.form = new FormGroup({
      title: new FormControl<string>(interview.title, []),
      candidateId: new FormControl<number>(interview.candidateId ?? 0, []),
      interviewDate: new FormControl<string>(interview.interviewDate, []),
      startTime: new FormControl<string>(interview.startTime, []),
      endTime: new FormControl<string>(interview.endTime, []),
      jobId: new FormControl<number>(interview.jobId ?? 0, []),
      interviewersId: new FormControl<number[]>(this.selectedInterviewersId, []),
      location: new FormControl<string>(interview.location ?? '', []),
      recruiterId: new FormControl<number>(interview.recruiterId ?? 0, []),
      meetingId: new FormControl<string>(interview.meetingId ?? '', []),
      note: new FormControl<string>(interview.note ?? '', []),
      result: new FormControl<string | null>(interview.result ?? null, []),
      status: new FormControl<string>(interview.status.toString(), []),
    });
  }

  public onSubmit() {
    if (this.form.invalid) {
      console.log('Invalid');
      return;
    }

    const data: InterviewModel = this.form.value;

    console.log(data);


    this.interviewService.update(this.interviewId, data).subscribe({
      next: (data) => {
        if (data) {
          console.log('Update success');
          this.toastr.success('Update success', 'Success')
          this.router.navigate(['/admin/interviews']);
        } else {
          console.log('Update failed');
        }
      },
      error: (error) => {
        this.toastr.error('Failed to update jobs', 'Error');
      }
    });
  }

  toggleDropdown(): void {
    this.dropdownVisible = !this.dropdownVisible;
  }

  public updateInterviewerSelected(id: number, name: string): void {
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
}
