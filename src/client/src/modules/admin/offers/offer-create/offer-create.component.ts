import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { OfferModel } from '../../../../models/offer/offer.model';

@Component({
  selector: 'app-offer-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './offer-create.component.html',
  styleUrl: './offer-create.component.css'
})
export class OfferCreateComponent implements OnInit {
  offerId: number | null = null;
  offerForm!: FormGroup;
  formSubmitted = false;
  
  public offer!: OfferModel;
  candidates: CandidateModel[] = [];
  approvers: UserForInputModel[] = [];
  interviews: InterviewModel[] = [];
  departments: any[] = [];
  recruiterOwner: UserForInputModel[] = [];
  positions: string[] = [];
  levels: LevelModel[] = [];

  constructor(
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    @Inject(OFFER_SERVICE) private offerService: IOffService,
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(INTERVIEW_SERVICE) private interviewService: IInterviewService,
    @Inject(DATA_FOR_INPUT_SERVICE)
        private readonly dataForInputService: IDataForInputService) {}

  ngOnInit(): void {
    // Lấy ID từ URL
    this.route.paramMap.subscribe(params => {
      this.offerId = Number(params.get('id'));
    });

    this.initFormCreate();

    if (this.offerId) {
      this.loadOfferEdit(this.offerId);
    }

    
    this.loadData();
  }

  loadOfferEdit(id: number) {
    this.offerService.getById(id).subscribe({
      next: (data) => {
        this.offer = data;
        console.log("Offer Detail:", this.offer);
  
        // Cập nhật form với dữ liệu nhận được
        this.offerForm.patchValue({
          id: this.offer.id,
          candidateId: this.offer.candidateId,
          interviewId: this.offer.interviewId,
          departmentId: this.offer.departmentId,
          position: this.offer.position,
          contractType: this.offer.contractType,
          contractStart: this.offer.contractStart,
          contractEnd: this.offer.contractEnd,
          status: this.offer.status,
          approvedBy: this.offer.approvedBy,
          salaryBasic: this.offer.salaryBasic,
          // levelId: this.offer.level,
          dueDate: this.offer.dueDate,
          note: this.offer.note
        });
      },
      error: (error) => {
        console.error('Lỗi khi lấy Offer:', error);
      }
    });
  }
  

  initFormCreate(): void {
    this.offerForm = this.fb.group({
      id: [''],
      candidateId: ['', Validators.required],
      interviewId: [''],
      departmentId: ['', Validators.required],
      position: ['', Validators.required],
      contractType: ['', Validators.required],
      contractStart: ['', Validators.required],
      contractEnd: ['', Validators.required],
      status: ['Waiting for Approval'],
      approvedBy: ['', Validators.required],
      recruiterId: ['', Validators.required],
      salaryBasic: ['', Validators.required],
      levelId: ['', Validators.required],
      dueDate: ['', Validators.required],
      note: ['']
    });
  }

  loadData(): void {
    this.candidateService.getAll().subscribe({
      next: (data) => {
        this.candidates = data.filter(candidate => candidate.status !== "BANNED");
        this.positions = [...new Set(data
          .map(candidate => candidate.currentPosition)
          .filter(item => item != null) as string[])];
      }
    });

    this.interviewService.getAll().subscribe({
      next: (data) => {
        this.interviews = data;
      }
    });

    this.dataForInputService.getUserForInputData(['MANAGER']).subscribe({
      next: (data) => {
        this.approvers = data;
      }
    });

    this.dataForInputService.getUserForInputData(['RECRUITER']).subscribe({
      next: (data) => {
        this.recruiterOwner = data;
      }
    });

    this.dataForInputService.getLevelData().subscribe({
      next: (data) => {
        this.levels = data;
      }
    });

    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      }
    });
  }

  submitOffer(): void {
    this.formSubmitted = true;
    
    if (this.offerForm.invalid) {
      this.markFormGroupTouched(this.offerForm);
      return;
    }

    const offerData = this.offerForm.value;
    console.log("Submitting offer data:", offerData);

    if (this.offerId) {
      // EDIT
      this.offerService.update(this.offerId, offerData).subscribe();
    } else {
      // CREATE
      this.offerService.create(offerData).subscribe();
    }

    this.router.navigate(['/admin/offers']);
  
    // this.offerService.create(offerData).subscribe({
    //   next: (response) => {
    //     console.log('Offer created successfully', response);
    //     // Navigate back to the offers list page
    //     this.router.navigate(['/admin/offers']);
    //   },
    //   error: (error) => {
    //     console.error('Error creating offer', error);
    //   }
    // });
  }

  // Cancel button handler
  cancelForm(): void {
    this.router.navigate(['/admin/offers']);
  }

  // Helper method to mark all controls as touched
  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  // Check if a control is invalid and has been touched or form submitted
  isFieldInvalid(fieldName: string): boolean {
    const control = this.offerForm.get(fieldName);
    return !!(control && control.invalid && (control.touched || this.formSubmitted));
  }
}