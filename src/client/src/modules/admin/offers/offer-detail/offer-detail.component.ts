import { Component, Inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OfferModel } from '../../../../models/offer/offer.model';
import { AUTH_SERVICE, CANDIDATE_SERVICE, COMMON_SERVICE, INTERVIEW_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { CommonModule } from '@angular/common';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ConfirmModalComponent } from '../../../modals/confirm-modal/confirm-modal.component';
import { ToastrService } from 'ngx-toastr';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { IAuthService } from '../../../../services/auth/auth-service.interface';

@Component({
  selector: 'app-offer-detail',
  imports: [RouterLink, CommonModule, ConfirmModalComponent],
  templateUrl: './offer-detail.component.html',
  styleUrl: './offer-detail.component.css'
})
export class OfferDetailComponent {
  public user: any;
  public isEditing: boolean = false;


  public offerId!: number;
  public offer!: OfferModel;
  public interviews!: InterviewModel[];
  public filteredInterviews!: InterviewModel | undefined;
  public usersInterviewer: UserForInputModel[] = [];
  public usersRecruiter: UserForInputModel[] = [];

  isModalOpen: boolean = false;

  constructor(
    private readonly toastr: ToastrService,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    private readonly route: ActivatedRoute,
    @Inject(OFFER_SERVICE) private readonly offerService: IOffService,
    @Inject(CANDIDATE_SERVICE) private readonly candidateService: ICandidateService,
    @Inject(COMMON_SERVICE)
        private readonly commonService: ICommonService,) {}

    ngOnInit(): void {
      this.authService.getUserInformation().subscribe((res) => {
        this.user = res;
      });

      // Lấy ID từ URL
      this.route.paramMap.subscribe(params => {
        this.offerId = Number(params.get('id'));
        this.loadOffer();
      });
      
      this.interviewService.getAll().subscribe({
        next: (data) => {
          this.interviews = data;
        }
      });

      this.commonService
        .getUserForInputData(['RECRUITER'])
        .subscribe((data) => {
          this.usersRecruiter = data;
      });

    }

    // Gọi API để lấy dữ liệu Offer
  loadOffer(): void {
    this.offerService.getById(this.offerId).subscribe({
      next: (data) => {
        this.offer = data;
        console.log('Offer Detail:', this.offer);
      },
      error: (error) => {
        console.error('Lỗi khi lấy Offer:', error);
      }
    });
  }

  get recruiter(): string {
    return this.usersRecruiter?.map(user => `${user.fullName} (${user.userName})`).join(', ') || '';
  }

  get interviewer(): string {
    this.filteredInterviews = this.interviews.find(i => i.id === this.offer.interviewId);

    return this.offer.interviewTitle + "<br>Interviewer: " + this.filteredInterviews?.interviewersName?.join(", ");
  }

  changeStatusOffer(status: string): void{
    const data: any = {
      id: this.offerId,
      status: status
    };
    this.offerService.changeStatus(data).subscribe();
  }

  changeStatusCandidate(status: string): void{
    const data: any = {
      id: this.offer.candidateId,
      status: status
    };
    this.candidateService.changeStatus(data).subscribe();
  }

  // Kiểm tra xem nút có được hiển thị không
  canShowButton(button: string): boolean {
    const status = this.offer?.status?.toLowerCase();  // Chuyển status thành chữ thường
    const roles: string[] = this.user.roles.map((role: string) => role.toLowerCase());  // Chuyển tất cả roles thành chữ thường

    console.log("roles: ", roles);
    console.log("status: ", status);

    // Quy tắc của các nút
    const buttonRules: { [key: string]: string[] } = {
      'Edit': ['waiting for approval'],
      'Accept': ['waiting for response'],
      'Approve': ['waiting for approval'],
      'Reject': ['waiting for approval'],
      'Cancel': ['waiting for approval', 'approved offer', 'waiting for response', 'accepted offer'],
      'Mark as sent to candidate': ['approved offer'],
      'Declined': ['waiting for response']
    };

    // Quy tắc vai trò cho các nút
    const roleRules: { [key: string]: string[] } = {
      'Edit': ['recruiter', 'manager', 'admin'],
      'Accept': ['recruiter', 'manager', 'admin'],
      'Approve': ['manager', 'admin'],
      'Reject': ['manager', 'admin'],
      'Cancel': ['recruiter', 'manager', 'admin'],
      'Mark as sent to candidate': ['recruiter', 'manager', 'admin'],
      'Declined': ['recruiter', 'manager', 'admin']
    };

    // Kiểm tra điều kiện có thỏa mãn không
    const canShow: boolean = buttonRules[button]?.includes(status) && roles.some(role => roleRules[button]?.includes(role));
    console.log("Can Show Button: ", canShow);

    return canShow;
  }


  openModal() {
    this.isModalOpen = true;
  }

  handleCloseModal() {
    this.isModalOpen = false;
  }

  handleConfirmCancel() {
    this.changeStatusOffer("Cancelled");
    this.changeStatusCandidate("CancelledOffer");
    this.isModalOpen = false;
  }
}
