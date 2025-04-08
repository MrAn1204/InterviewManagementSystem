import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InterviewModel, InterviewStatus } from '../../../../models/interview/interview.model';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InterviewTableComponent } from '../interview-table/interview-table.component';
import { COMMON_SERVICE, INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { ToastrService } from 'ngx-toastr';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';

@Component({
  selector: 'app-interview-list',
  imports: [RouterLink, CommonModule, FontAwesomeModule, InterviewTableComponent, FormsModule, ReactiveFormsModule],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.css'
})
export class InterviewListComponent extends MasterDataListComponent<InterviewModel> {
  public interviewerList!: UserForInputModel[];
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
    private readonly toastr: ToastrService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    @Inject(COMMON_SERVICE) private readonly commonService: ICommonService) {
    super();
  }

  override ngOnInit(): void {
    this.headerService.setTitle('Interview');
    this.interviewService.search(this.filter).subscribe((res) => {
      this.data = res;
    });
    this.commonService.getUserForInputData(['INTERVIEWER'])
      .subscribe((res) => this.interviewerList = res);
    this.createForm();
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl<string>(''),
      status: new FormControl<string>(''),
      interviewerId: new FormControl<number>(0),
    });
  }

  public override searchData(): void {
    Object.assign(this.filter, this.searchForm.value);
    this.interviewService.search(this.filter).subscribe({
      next: (response) => {
        this.data = response;
      },
      error: (error) => {
        this.toastr.error('Failed to load interviews', 'Error');
        console.error('Error loading interviews:', error);
      }
    });
  }
}
