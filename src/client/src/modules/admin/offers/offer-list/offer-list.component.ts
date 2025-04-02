import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { DATA_FOR_INPUT_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { OfferModel } from '../../../../models/offer/offer.model';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-offer-list',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.css'
})
export class OfferListComponent {
  // public filter: SearchModel = {
  //   keyword: '',
  //   departmentName: '',
  //   candidateStatus,
  //   status: '',
  //   pageNumber: 1,
  //   pageSize: 5,
  //   orderBy: '',
  //   orderDirection: OrderDirection.ASC,
  // };
  public keyword!: string;
  public departmentName!: string;
  public candidateStatus!: string;
  public currentPage: number = 1;
  public currentPageSize: number = 5;
  public pageSizeOptions: number[] = [5, 10, 20, 50];

  public searchForm!: FormGroup;
  public data!: OfferModel[];
  
  constructor(
    private fb: FormBuilder,
    private headerService: HeaderService,
    @Inject(OFFER_SERVICE) private offerService: IOffService,
  ) { }

  ngOnInit(): void {
    this.headerService.setTitle('Offer');
    this.getAllOffers();

    // Khởi tạo form
    this.searchForm = this.fb.group({
      keyword: [''],
      departmentName: [''],
      candidateStatus: [''],
    });
  }

  getAllOffers(): void {
    this.offerService.getAll().subscribe({
      next: (response) => {
        this.data = response; // Gán dữ liệu trả về vào biến data
        console.log('Danh sách Offers:', this.data);
      },
      error: (error) => {
        console.error('Lỗi khi lấy danh sách Offer:', error);
      }
    });
  }


  // Hàm lấy giá trị departmentName và status từ form
  onSearch(): void {
    const filterData = this.searchForm.value;
    console.log('Department:', filterData.departmentName);
    console.log('Status:', filterData.candidateStatus);
    console.log('Form Values:', this.searchForm.value);

    console.log('Filter Data being sent to API:', filterData);

    this.offerService.search(filterData).subscribe({
      next: (response) => {
        this.data = response;  // Gán lại dữ liệu theo kết quả tìm kiếm
        console.log('Kết quả tìm kiếm:', this.data);
      },
      error: (error) => {
        console.error('Lỗi khi tìm kiếm Offer:', error);
      }
    });
  }
}
