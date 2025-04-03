import { Component, Inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OfferModel } from '../../../../models/offer/offer.model';
import { DATA_FOR_INPUT_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { IDataForInputService } from '../../../../services/data-for-input/data-for-input-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';

@Component({
  selector: 'app-offer-detail',
  imports: [RouterLink],
  templateUrl: './offer-detail.component.html',
  styleUrl: './offer-detail.component.css'
})
export class OfferDetailComponent {
  public offerId!: number;
  public offer!: OfferModel;
  public usersInterviewer: UserForInputModel[] = [];
  public usersRecruiter: UserForInputModel[] = [];

  constructor(
    private route: ActivatedRoute, 
    @Inject(OFFER_SERVICE) private offerService: IOffService,
    @Inject(DATA_FOR_INPUT_SERVICE)
        private readonly dataForInputService: IDataForInputService,) {}
  
    ngOnInit(): void {
      // Lấy ID từ URL
      this.route.paramMap.subscribe(params => {
        this.offerId = Number(params.get('id'));
        this.loadOffer();
      });

      this.dataForInputService
        .getUserForInputData(['INTERVIEWER'])
        .subscribe((data) => {
          this.usersInterviewer = data;
      });

      this.dataForInputService
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
    return this.offer.interviewTitle + "\nInterviewer: "
     + this.usersInterviewer?.map(user => user.userName).join(', ') || '';
  }
}
