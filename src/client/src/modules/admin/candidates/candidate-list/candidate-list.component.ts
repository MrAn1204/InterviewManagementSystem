import { Component, Inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  CANDIDATE_SERVICE,
  DATA_FOR_INPUT_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { IDataForInputService } from '../../../../services/data-for-input/data-for-input-service.interface';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-candidate-list',
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css',
})
export class CandidateListComponent implements OnInit {
  public filter: SearchModel = {
    keyword: '',
    status: '',
    pageNumber: 1,
    pageSize: 5,
    orderBy: 'CreatedDate',
    orderDirection: OrderDirection.DESC,
  };
  public currentPage: number = 1;
  public currentPageSize: number = 5;
  public pageSizeOptions: number[] = [5, 10, 20, 50];

  public searchForm!: FormGroup;

  public data!: PaginatedResult<CandidateModel>;
  public statusList!: CandidateStatusModel[];

  constructor(
    private readonly headerService: HeaderService,
    @Inject(CANDIDATE_SERVICE)
    private readonly candidateService: ICandidateService,
    @Inject(DATA_FOR_INPUT_SERVICE)
    private readonly dataForInputService: IDataForInputService,
    private readonly toastService: ToastrService
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.headerService.setTitle('Candidate');
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
    this.dataForInputService.getAllCandidateStatus().subscribe((res) => {
      this.statusList = res;
    });
  }

  public search(): void {
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
  }

  private createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl(''),
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

  public pageChange(direction: number): void {
    if (direction < 0) {
      if (this.filter.pageNumber <= 1) {
        return;
      }
      this.filter.pageNumber -= 1;
    } else {
      if (this.filter.pageNumber >= this.data.totalPages) {
        return;
      }
      this.filter.pageNumber += 1;
    }
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
  }

  public deleteItem(id: number): void {
    this.candidateService.delete(id).subscribe({
      next: (res) => {
        this.data.items = this.data.items.filter((item) => item.id != id);
        this.toastService.success('Delete successful!', 'Success');
      },
      error: () => {
        this.toastService.error('Delete unsuccessful!', 'Error');
      },
    });
  }
}
