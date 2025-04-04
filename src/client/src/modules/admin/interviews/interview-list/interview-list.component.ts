import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InterviewModel, InterviewResult, InterviewStatus } from '../../../../models/interview/interview.model';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InterviewTableComponent } from '../table/table.component';
import { TableColumn } from '../table/table-column.model';
import { INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { MasterDataListComponent } from '../../master-data/master-data.component';

@Component({
  selector: 'app-interview-list',
  imports: [RouterLink, CommonModule, FontAwesomeModule, InterviewTableComponent, FormsModule, ReactiveFormsModule],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.css'
})
export class InterviewListComponent extends MasterDataListComponent<InterviewModel> {
  public interviewerList = [
    {
      id: 2,
      username: 'tranthib',
      fullname: 'Trần Thị B'
    },
    {
      id: 3,
      username: 'imsG2',
      fullname: 'John Doe'
    }
  ];
  public statusList: string[] = Object.keys(InterviewStatus).filter(key => isNaN(Number(key)));

  public override columns: TableColumn[] = [
    { name: "Title", value: "title" },
    { name: "Candidate Name", value: "candidateName" },
    { name: "Interviewers", value: "interviewersName" },
    { name: "Schedule", value: "schedule" },
    { name: "Result", value: "result" },
    { name: "Status", value: "status" },
    { name: "Job", value: "jobName" },
  ];

  constructor(
    private readonly headerService: HeaderService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService) {
    super();
  }

  override ngOnInit(): void {
    this.headerService.setTitle('Interview');
    this.interviewService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
    this.createForm();
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl<string>(''),
      status: new FormControl<string>(''),
      interviewerId: new FormControl<number>(0),
    });
  }

  public search(): void {
    Object.assign(this.filter, this.searchForm.value);
    this.interviewService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
  }

  public override onPageChange(page: number): void {
    this.filter.pageNumber = page;
    if (page < 0 || page > this.data.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.searchData();
  }

  public override onPageSizeChange(event: any): void {
    this.filter.pageSize = event.target.value;
    this.searchData();
  }
}
