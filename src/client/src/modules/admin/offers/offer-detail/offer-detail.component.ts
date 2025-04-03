import { Component, Inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OfferModel } from '../../../../models/offer/offer.model';
import { OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';

@Component({
  selector: 'app-offer-detail',
  imports: [RouterLink],
  templateUrl: './offer-detail.component.html',
  styleUrl: './offer-detail.component.css'
})
export class OfferDetailComponent {
  offerId!: number;
  offer!: OfferModel;

  constructor(
    private route: ActivatedRoute, 
    @Inject(OFFER_SERVICE) private offerService: IOffService,) {}
  
    ngOnInit(): void {
      // Lấy ID từ URL
      this.route.paramMap.subscribe(params => {
        this.offerId = Number(params.get('id'));
        this.loadOffer();
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
}
