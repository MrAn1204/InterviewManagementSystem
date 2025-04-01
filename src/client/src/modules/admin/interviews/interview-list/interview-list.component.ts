import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { OrderDirection, SearchModel } from '../../../../models/search.model';
import { FormGroup } from '@angular/forms';
import { InterviewModel, InterviewResult, InterviewStatus } from '../../../../models/interview/interview.model';
import { PaginatedResult } from '../../../../models/candidate/paginated-result.model';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TableComponent } from '../../../shared/common/table/table.component';
import { TableColumn } from '../../../shared/common/table/table-column.model';

@Component({
  selector: 'app-interview-list',
  imports: [RouterLink, CommonModule, FontAwesomeModule, TableComponent],
  templateUrl: './interview-list.component.html',
  styleUrl: './interview-list.component.css'
})
export class InterviewListComponent {
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
  public isShowNumber?: boolean = true

  public searchForm!: FormGroup;

  public columns: TableColumn[] = [
    { name: "Title", value: "name" },
    { name: "Candidate Name", value: "candidate" },
    { name: "Interviewers", value: "interviewers" },
    { name: "Schedule", value: "schedule" },
    { name: "Result", value: "result" },
    { name: "Status", value: "status" },
    { name: "Job", value: "job" },
  ];

  public data: PaginatedResult<InterviewModel> = {
    items: [
      {
        title: "Senior Java Developer",
        candidate: "John Doe",
        interviewers: ["John Doe", "Jane Doe"],
        schedule: "2023-01-01 10:00 AM",
        result: InterviewResult.Passed,
        status: InterviewStatus.New,
        job: "Software Engineer",
        location: "New York",
        createdBy: "John Doe",
        recruiter: "Jane Doe", 
      }
    ],
    pageNumber: 1,
    pageSize: 5,
    totalCount: 1,
    totalPages: 1
  }

  constructor(private headerService: HeaderService){}
  
    ngOnInit(): void {
      this.headerService.setTitle('Interview');
    }
  
}
