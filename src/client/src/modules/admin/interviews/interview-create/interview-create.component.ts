import { Component, HostListener, inject, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AUTH_SERVICE, CANDIDATE_SERVICE, INTERVIEW_SERVICE, JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { IJobService } from '../../../../services/job/job-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { ToastrService } from 'ngx-toastr';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { UserService } from '../../../../services/user/user.service';
import { User } from '../../../../models/User';

@Component({
  selector: 'app-interview-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './interview-create.component.html',
  styleUrl: './interview-create.component.css'
})
export class InterviewCreateComponent implements OnInit {
  public form!: FormGroup;
  public interviewerInput!: number[];
  public selectedInterviewers: string[] = [];
  public dropdownVisible = false;

  private readonly selectedInterviewersId: number[] = [];

  public interviewerList!: User[];

  public jobList!: JobModel[];

  public recruiterList!: User[];

  public candidateList!: CandidateModel[];

  constructor(
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    @Inject(CANDIDATE_SERVICE) private readonly candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly toastr: ToastrService
  ) {
    this.userService = inject(UserService);
  }

  ngOnInit(): void {
    this.createForm();
    this.candidateService.getAll().subscribe((res) => this.candidateList = res);
    this.jobService.getAll().subscribe((res) => this.jobList = res);
    
    this.userService.getUsers({ pageNumber: 1, pageSize: 100 })
      .subscribe((res) => this.recruiterList = res.items.filter(user => user.roles.includes('RECRUITER')));
    this.userService.getUsers({ pageNumber: 1, pageSize: 100 })
      .subscribe((res) => this.interviewerList = res.items.filter(user => user.roles.includes('INTERVIEWER')));
  }

  public createForm() {
    this.form = new FormGroup({
      title: new FormControl<string>('', []),
      candidateId: new FormControl<number>(0, []),
      interviewDate: new FormControl<string>('', []),
      startTime: new FormControl<string>('', []),
      endTime: new FormControl<string>('', []),
      jobId: new FormControl<number>(0, []),
      interviewersId: new FormControl<number[]>([], []),
      location: new FormControl<string>('', []),
      recruiterId: new FormControl<number>(0, []),
      meetingId: new FormControl<string>('', []),
      note: new FormControl<string>('', []),
    });
  }

  public onSubmit() {
    if (this.form.invalid) {
      console.log('Invalid');
      return;
    }

    const data: InterviewModel = this.form.value;

    this.interviewService.create(data).subscribe({
      next: (data) => {
        if (data) {
          console.log('Create success');
          this.toastr.success('Create success', 'Success')
          this.router.navigate(['/admin/interviews']);
        } else {
          console.log('Create failed');
        }
      },
      error: (error) => {
        this.toastr.error('Failed to create jobs', 'Error');
        console.error('Error create jobs:', error);
      },
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
