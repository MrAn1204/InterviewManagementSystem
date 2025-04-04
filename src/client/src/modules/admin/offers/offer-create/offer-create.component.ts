import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CANDIDATE_SERVICE, DATA_FOR_INPUT_SERVICE, INTERVIEW_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { IDataForInputService } from '../../../../services/data-for-input/data-for-input-service.interface';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { DepartmentService } from '../../../../services/department/department.service';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-offer-create',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './offer-create.component.html',
  styleUrl: './offer-create.component.css'
})
export class OfferCreateComponent {
  candidates!: CandidateModel[];
  approvers!: UserForInputModel[];
  interviews!: InterviewModel[];
  departments: any[] = [];
  recruiterOwner!: UserForInputModel[];
  positions!: string[];
  levels!: LevelModel[];

  selectedCandidate!: number;
  selectedApprover!: number;
  selectedInterview!: number;
  selectedLevel!:number;
  selectedDepartment!: number;
  selectedRecruiter!: number;
  selectedPosition!: string;
  selectedStatus!: string;
  fromDate!: string;
  toDate!: string;
  dueDate!: string;
  salary!: string;
  note!: string;
  selectedContractType!: string;

  constructor(
    private departmentService: DepartmentService,
    @Inject(OFFER_SERVICE) private offerService: IOffService,
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(INTERVIEW_SERVICE) private interviewService: IInterviewService,
    @Inject(DATA_FOR_INPUT_SERVICE)
        private readonly dataForInputService: IDataForInputService,) {}


  ngOnInit(): void {
    this.candidateService.getAll().subscribe({
      next: (data) => {
        this.candidates = data;
      }
    })

    this.candidateService.getAll().subscribe({
      next: (data) => {
        this.positions = data.map(candidate => candidate.currentPosition).filter(item => item != null) as string[];
      }
    })

    this.interviewService.getAll().subscribe({
      next: (data) => {
        this.interviews = data;
      }
    })

    this.dataForInputService.getUserForInputData(['MANAGER']).subscribe({
      next: (data) => {
        this.approvers = data;
      }
    })

    this.dataForInputService.getUserForInputData(['RECRUITER']).subscribe({
      next: (data) => {
        this.recruiterOwner = data;
      }
    })

    this.dataForInputService.getLevelData().subscribe({
      next: (data) => {
        this.levels = data;
      }
    })

    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      }
    })
  }

  submitOffer() {
    const offerData = {
      candidateId: this.selectedCandidate,
      interviewId: this.selectedInterview,
      departmentId: this.selectedDepartment,
      position: this.selectedPosition,
      contractType: this.selectedContractType,
      contractStart: this.fromDate,
      contractEnd: this.toDate,
      status: this.selectedStatus,
      approver: this.selectedApprover,
      recruiter: this.selectedRecruiter,
      approvedDate: '',
      salary: this.salary,
      note: this.note,
      dueDate: this.dueDate,
    };
  
    this.offerService.create(offerData).subscribe({
      next: (response) => {
        console.log('Offer created successfully', response);
      },
      error: (error) => {
        console.error('Error creating offer', error);
      }
    });
  }
  

}
