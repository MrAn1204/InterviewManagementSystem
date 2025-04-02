import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { DATA_FOR_INPUT_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import { FormGroup } from '@angular/forms';
import { OfferModel } from '../../../../models/offer/offer.model';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';

@Component({
  selector: 'app-offer-list',
  imports: [RouterLink],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.css'
})
export class OfferListComponent {
  public filter: SearchModel = {
    keyword: '',
    status: '',
    pageNumber: 1,
    pageSize: 5,
    orderBy: '',
    orderDirection: OrderDirection.ASC,
  };
  public currentPage: number = 1;
  public currentPageSize: number = 5;
  public pageSizeOptions: number[] = [5, 10, 20, 50];

  public searchForm!: FormGroup;

  public data!: PaginatedResult<OfferModel>;
  
  constructor(private headerService: HeaderService,
    @Inject(OFFER_SERVICE) private offerService: IOffService,
  ) { }

  ngOnInit(): void {
    this.headerService.setTitle('Offer');
  }

}
