import { Component, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { JobStatusModel } from '../../../../models/job/job-status.model';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-job-list',
  imports: [RouterLink, CommonModule, ReactiveFormsModule,
    FormsModule,
    TableComponent,
    FontAwesomeModule
  ],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent
  extends MasterDataListComponent<JobModel>
  implements OnInit {
    
  public statusList: JobStatusModel[] = [{ id: 1, name: 'Draft' },
     { id: 2, name: 'Open' }, { id: 3, name: 'Closed' }];

  public override columns: TableColumn[] = [
    { name: 'Job Title', value: 'title' },
    { name: 'Required Skills', value: 'skillsDisplay', formatter: this.formatSkills.bind(this) },
    { name: 'Start Date', value: 'startDate', formatter: this.formatDate.bind(this) },
    { name: 'End Date', value: 'endDate', formatter: this.formatDate.bind(this) },
    { name: 'Level', value: 'levelsDisplay', formatter: this.formatLevels.bind(this) },
    { name: 'Status', value: 'status' }
  ];
  

  constructor(
    private readonly headerService: HeaderService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly toastr: ToastrService,
    private readonly router: Router
  ) {
    super();
  }



  public override ngOnInit(): void {
    this.createForm();
    this.headerService.setTitle('Job');
    this.searchData();
  }


  protected override searchData(): void {
    this.jobService.search(this.filter).subscribe({
      next: (response) => {
        this.data = response;
      },
      error: (error) => {
        this.toastr.error('Failed to load jobs', 'Error');
        console.error('Error loading jobs:', error);
      }
    });
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl(''),
    });
  }

  private formatSkills(job: JobModel): string {
    return job.skills?.map(skill => skill.skillName).join(', ') || 'N/A';
  }

  private formatLevels(job: JobModel): string {
    return job.levels?.map(level => level.levelName).join(', ') || 'N/A';
  }

  private formatDate(job: JobModel, column: TableColumn): string {
    const dateValue = job[column.value as keyof JobModel];
    if (dateValue) {
      const formatter = new Intl.DateTimeFormat('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
      });
      const date = new Date(dateValue as string);
      return formatter.format(date);
    }
    return 'Invalid Date';
  }

  public keywordChange(): void {
    this.filter.keyword = this.searchForm.get('keyword')?.value || '';
  }

  public statusChange(): void {
    this.filter.status = this.searchForm.get('status')?.value || '';
  }

  public delete(id: number): void {
    this.jobService.delete(id).subscribe((data) => {
      if (data) {
        this.searchData();
      }
    });
  }  

  public edit(id: number): void {
    setTimeout(() => {      
      this.router.navigate(['/admin/jobs', id, 'edit']);
    }, 150);
  }

  public create(): void {
    setTimeout(() => {
      this.router.navigate(['/admin/jobs/create']);
    }, 150);
  }

  public viewDetail(id: number): void {
    this.router.navigate(['/admin/jobs', id, 'detail']);
  }
}
