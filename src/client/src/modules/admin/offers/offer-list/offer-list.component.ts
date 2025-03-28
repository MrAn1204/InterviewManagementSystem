import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';

@Component({
  selector: 'app-offer-list',
  imports: [RouterLink],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.css'
})
export class OfferListComponent {
  constructor(private headerService: HeaderService) { }

  ngOnInit(): void {
    this.headerService.setTitle('Offer');
  }

}
