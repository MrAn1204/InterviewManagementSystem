import { Component, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  CANDIDATE_SERVICE,
  COMMON_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { CommonModule } from '@angular/common';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import { ToastrService } from 'ngx-toastr';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TableComponent } from '../../../../core/components/table/table.component';
import { log } from 'console';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-candidate-list',
  imports: [
    RouterLink,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    TableComponent,
    FontAwesomeModule,
  ],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css',
})
export class CandidateListComponent
  extends MasterDataListComponent<CandidateModel>
  implements OnInit
{
  public override columns: TableColumn[] = [
    { name: 'Name', value: 'fullName' },
    { name: 'Email', value: 'email' },
    { name: 'Phone No.', value: 'phoneNumber' },
    { name: 'Current Position', value: 'currentPosition' },
    {
      name: 'Owner Hr',
      value: 'recruiter',
      formatter: this.formatRecruiter.bind(this),
    },
    { name: 'Status', value: 'status' },
  ];

  public statusList!: CandidateStatusModel[];

  constructor(
    private readonly headerService: HeaderService,
    public readonly authService: AuthService,
    @Inject(CANDIDATE_SERVICE)
    private readonly candidateService: ICandidateService,
    @Inject(COMMON_SERVICE)
    private readonly commonService: ICommonService,
    private readonly toastService: ToastrService,
    private readonly router: Router
  ) {
    super();
  }

  public override ngOnInit(): void {
    this.createForm();
    this.headerService.setTitle('Candidate');
    this.searchData();
    this.commonService.getAllCandidateStatus().subscribe((res) => {
      this.statusList = res;
    });
  }

  public override searchData(): void {
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
  }

  private formatRecruiter(candidate: CandidateModel): string {
    console.log();
    return candidate.recruiter.userName;
  }

  public override createForm(): void {
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

  public delete(id: number): void {
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

  public edit(id: number): void {
    setTimeout(() => {
      this.router.navigate(['/admin/candidates', id, 'edit']);
    }, 150);
  }

  public create(): void {
    setTimeout(() => {
      this.router.navigate(['/admin/candidates/create']);
    }, 150);
  }

  public viewDetail(id: number): void {
    this.router.navigate(['/admin/candidates', id, 'detail']);
  }
}
