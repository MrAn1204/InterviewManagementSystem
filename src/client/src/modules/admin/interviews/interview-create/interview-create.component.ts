import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InterviewModel } from '../../../../models/interview/interview.model';

@Component({
  selector: 'app-interview-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './interview-create.component.html',
  styleUrl: './interview-create.component.css'
})
export class InterviewCreateComponent implements OnInit {
  public form!: FormGroup;
  public interviewerInput!: number[];
  public interviewerList = ['Thiệu P', 'Name B', 'John H'];
  public selectedInterviewers: string[] = [];
  public dropdownVisible = false;

  constructor(
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
  ) { }

  ngOnInit(): void {
    this.createForm();
  }

  public createForm() {
    this.form = new FormGroup({
      title: new FormControl('', []),
      candidateId: new FormControl(0, []),
      interviewDate: new FormControl('', []),
      startTime: new FormControl('', []),
      endTime: new FormControl('', []),
      jobId: new FormControl(0, []),
      interviewersId: new FormControl([], []),
      location: new FormControl('', []),
      recruiterId: new FormControl(0, []),
      meetingId: new FormControl('', []),
      note: new FormControl('', []),
    });
  }

  public onSubmit() {
    if (this.form.invalid) {
      console.log('Invalid');
      return;
    }

    const data: InterviewModel = this.form.value;

    this.interviewService.create(data).subscribe((res) => {
      if (res) {
        console.log('Create success');
      } else {
        console.log('Create failed');
      }
    });
  }
  
  toggleDropdown(): void {
    this.dropdownVisible = !this.dropdownVisible;
  }

  public updateInterviewerSelected(interviewer: string): void {
    const index = this.selectedInterviewers.indexOf(interviewer);
    if (index === -1) {
      this.selectedInterviewers.push(interviewer); // Add to selected interviewers
    } else {
      this.selectedInterviewers.splice(index, 1); // Remove from selected interviewers
    }

    this.form.patchValue({
      interviewersId: this.selectedInterviewers.join(', ')
    });

    console.log(this.selectedInterviewers);
    
  }

  @HostListener('document:click', ['$event'])
  closeDropdownOnClickOutside(event: MouseEvent): void {
    const dropdownElement = document.getElementById('interviewerDropdown');
    const inputElement = document.getElementById('interviewersId');
    
    // Check if the clicked element is outside the dropdown and the input element
    if (dropdownElement && inputElement && !dropdownElement.contains(event.target as Node) && !inputElement.contains(event.target as Node)) {
      this.dropdownVisible = false;
    }
  }
}
