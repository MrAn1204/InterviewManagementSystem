import { Component, Inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
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
  public statusList: JobStatusModel[] = [{ id: 1, name: 'Draft' }, { id: 2, name: 'Open' }, { id: 3, name: 'Closed' }];
  public override columns: TableColumn[] = [
    { name: 'Job Title', value: 'title' },
    { name: 'Required Skills', value: 'skillsDisplay', formatter: this.formatSkills.bind(this) },
    { name: 'Start Date', value: 'startDate' },
    { name: 'End Date', value: 'endDate' },
    { name: 'Level', value: 'levelsDisplay', formatter: this.formatLevels.bind(this) },
    { name: 'Status', value: 'status' }
  ];

  constructor(
    private headerService: HeaderService,
    @Inject(JOB_SERVICE) private jobService: IJobService,
    private toastr: ToastrService,
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
        // Process each job item to ensure display properties
        const processedItems = response.items.map(job => {
          return {
            ...job,
            skillsDisplay: this.formatSkills(job),
            levelsDisplay: this.formatLevels(job)
          };
        });

        this.data = {
          ...response,
          items: processedItems
        };
      },
      error: (error) => {
        this.toastr.error('Failed to load jobs', 'Error');
        console.error('Error loading jobs:', error);
      }
    });
  }

  public getSkillNames(job: JobModel): string {
    return job.skills?.map(skill => skill.skillName).join(', ') || '';
  }

  public getLevelNames(job: JobModel): string {
    return job.levels?.map(level => level.levelName).join(', ') || '';
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl(''),
    });
  }

  formatSkills(job: JobModel): string {    
    return job.skills?.map(skill => skill.skillName).join(', ') || 'N/A';
  }  

  formatLevels(job: JobModel): string {
    return job.levels?.map(level => level.levelName).join(', ') || 'N/A';
  }

  public keywordChange(): void {
    this.filter.keyword = this.searchForm.get('keyword')?.value || '';
  }

  public statusChange(): void {
    this.filter.status = this.searchForm.get('status')?.value || '';
  }

  public delete(id: number): void {
    this.jobService.delete(id).subscribe((data) => {
      // Neu xoa duoc thi goi lai ham getData de load lai du lieu
      if (data) {
        this.searchData();
      }
    });
  }
  search(): void {
    this.currentPage = 1; // Reset to first page on new search
    this.searchData();
  }

  handlePageChange(page: number): void {
    this.currentPage = page;
    this.searchData();
  }

  handlePageSizeChange(event: any): void {
    this.currentPageSize = event.target.value;
    this.currentPage = 1; // Reset to first page when changing page size
    this.searchData();
  }

  public edit(id: number): void {
    setTimeout(() => {
      this.selectedItem = this.data.items.find((x) => x.id === id);
      // Scroll into view
    }, 150);
  }

  public create(): void {
    setTimeout(() => {
      this.selectedItem = null;
      // Scroll into view
    }, 150);
  }
}
