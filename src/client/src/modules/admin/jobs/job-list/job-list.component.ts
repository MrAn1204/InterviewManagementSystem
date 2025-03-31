import { Component, Inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { SearchModel, OrderDirection } from '../../../../models/search.model';
import { JobModel } from '../../../../models/job/job.model';
import { JobStatusModel } from '../../../../models/job/job-status.model';

@Component({
  selector: 'app-job-list',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent {
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

  public data!: JobModel[];
  public statusList!: JobStatusModel[];

  constructor(
    private headerService: HeaderService,
    @Inject(JOB_SERVICE) private jobService: IJobService,
  ) { }

  ngOnInit(): void {
    this.createForm();
    this.headerService.setTitle('Job');
    this.jobService.getAll().subscribe((res) => {
      this.data = res.map(job => ({
        ...job,
        skillNames: job.skills ? job.skills.map(skill => skill.skillName).join(', ') : 'N/A',
        levelNames: job.levels ? job.levels.map(level => level.levelName).join(', ') : 'N/A',
      }));
    });
    console.log(this.data);
  }

  // public search(): void {
  //   this.jobService.search(this.filter).subscribe((res) => {
  //     this.data = res;
  //   });
  // }

  private createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl(''),
    });
  }

  // public keywordChange(): void {
  //   this.filter.keyword = this.searchForm.value.keyword;
  // }

  // public statusChange(): void {
  //   this.filter.status = this.searchForm.value.status;
  // }

  // public pageChange(direction: number): void {
  //   if (direction < 0) {
  //     this.filter.pageNumber -= 1;
  //   } else {
  //     this.filter.pageNumber += 1;
  //   }
  //   this.jobService.search(this.filter).subscribe((res) => {
  //     this.data = res;
  //   });
  // }

}
