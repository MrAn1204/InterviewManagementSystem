import { MasterDataListComponent } from './../../master-data/master-data.component';
import { Component, Inject } from '@angular/core';
import { Route, Router, RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { COMMON_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { IOffService } from '../../../../services/offer/offer-service.interface';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { OfferModel } from '../../../../models/offer/offer.model';
import { CommonModule } from '@angular/common';
import { ExportOfferModalComponent } from '../../../modals/export-offer-modal/export-offer-modal.component';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { DepartmentService } from '../../../../services/department/department.service';
import { TableComponent } from "../../../../core/components/table/table.component";
import { TableColumn } from '../../../../core/models/table/table-column.model';

@Component({
  selector: 'app-offer-list',
  imports: [RouterLink, CommonModule, ReactiveFormsModule, ExportOfferModalComponent, TableComponent],
  templateUrl: './offer-list.component.html',
  styleUrl: './offer-list.component.css'
})
export class OfferListComponent extends MasterDataListComponent<OfferModel> {

  public override columns: TableColumn[] = [
      { name: 'Candidate Name', value: 'candidateName' },
      { name: 'Email', value: 'candidateEmail' },
      { name: 'Approver', value: 'approver' },
      { name: 'Department', value: 'departmentName' },
      { name: 'Notes', value: 'note' },
      { name: 'Status', value: 'status' },
    ];

  public statusList!: CandidateStatusModel[];
  public departments: any[] = [];
  isExportModalOpen = false;

  openExportModal() {
    this.isExportModalOpen = true;
    console.log('Modal Open:', this.isExportModalOpen);
  }

  closeExportModal() {
    this.isExportModalOpen = false;
    console.log('Modal Close:', this.isExportModalOpen);
  }
  
  constructor(
    private fb: FormBuilder,
    private headerService: HeaderService,
    private router: Router,
    private departmentService: DepartmentService,
    @Inject(OFFER_SERVICE) private offerService: IOffService,
    @Inject(COMMON_SERVICE) private commonService: ICommonService,
  ) {
    super();
  }

  override ngOnInit(): void {
    this.headerService.setTitle('Offer');
    // this.getAllOffers();

    this.commonService.getAllCandidateStatus().subscribe((res) => {
      this.statusList = res;
    });

    // Khởi tạo form
    this.searchForm = this.fb.group({
      keyword: [''],
      status: [''],
      departmentName: [''],
    });

    this.searchData();

    this.departmentService.getAllDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      }
    });

    
  }

  // getAllOffers(): void {
  //   this.offerService.getAll().subscribe({
  //     next: (response) => {
  //       this.data = response; // Gán dữ liệu trả về vào biến data
  //       console.log('Danh sách Offers:', this.data);
  //     },
  //     error: (error) => {
  //       console.error('Lỗi khi lấy danh sách Offer:', error);
  //     }
  //   });
  // }


  // Hàm lấy giá trị departmentName và status từ form
  override searchData(): void {
    console.log('Filter:', this.filter);
    this.offerService.search(this.filter).subscribe({
      next: (response) => {
        this.data = response;
        console.log('Kết quả tìm kiếm:', this.data.items);
      },
      error: (error) => {
        console.error('Lỗi khi tìm kiếm Offer:', error);
      }
    });
  }

  public keywordChange(): void {
    console.log(this.searchForm.value.keyword);
    this.filter.keyword = this.searchForm.value.keyword;
  }

  public statusChange(): void {
    console.log(this.searchForm.value.status);
    this.filter.status = this.searchForm.value.status;
  }

  public departmentChange(): void {
    console.log(this.searchForm.value.departmentName);
    this.filter.departmentName = this.searchForm.value.status;
  }

  public edit(id: number): void {
    setTimeout(() => {
      this.router.navigate(['/admin/offers', id, 'edit']);
    }, 150);
  }

  public viewDetail(id: number): void {
    this.router.navigate(['/admin/offers', id, 'detail']);
  }
}
