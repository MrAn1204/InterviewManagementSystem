import { Component, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HeaderService } from '../../../../services/header/header.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AUTH_SERVICE, JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { JobStatusModel } from '../../../../models/job/job-status.model';
import { MasterDataListComponent } from '../../master-data/master-data.component';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ToastrService } from 'ngx-toastr';
import { JobImportResult } from '../../../../models/job/job-import-result.model';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { AuthService } from '../../../../services/auth/auth.service';

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

  public selectedFile: File | null = null;
  public importResult: JobImportResult | null = null;
  public isImporting: boolean = false;
  public importErrors: string[] = [];
  private createdBy!: number;

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
    @Inject(AUTH_SERVICE) public readonly authService: IAuthService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly toastr: ToastrService,
    private readonly router: Router
  ) {
    super();
  }

  public override ngOnInit(): void {
    this.createForm();
    this.headerService.setTitle('Job');
    this.authService.getUserInformation().subscribe((user) => {
      this.createdBy = user?.id ? Number(user.id) : 1;
    });
    this.searchData();
  }

  public onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const allowedTypes = ['.xlsx'];
      const fileExt = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();

      if (allowedTypes.includes(fileExt)) {
        this.selectedFile = file;
        this.importResult = null;
        this.importErrors = [];

        this.importJobs();
      } else {
        this.toastr.error('Please select a valid file (.xlsx)', 'Invalid File');
        event.target.value = '';
        this.selectedFile = null;
      }
    }
  }

  public importJobs(): void {
    if (!this.selectedFile) {
      this.toastr.warning('Please select a file to import.', 'No File Selected');
      return;
    }

    this.isImporting = true;
    this.importResult = null;
    this.importErrors = [];

    this.jobService.importJobs(this.selectedFile, this.createdBy).subscribe({
      next: (result) => {
        this.importResult = result;
        this.isImporting = false;

        if (result.errors && result.errors.length > 0) {
          this.importErrors = result.errors;
          this.toastr.warning(`Import completed with ${result.errors.length} errors`, 'Import Warning');
        } else {
          this.toastr.success(`Successfully imported ${result.importedRows || 0} jobs
            <br>Skipped ${result.skippedRows} existing jobs`
            , 'Import Success',
            { enableHtml: true }
          );
          this.searchData();

          this.selectedFile = null;
          const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
          if (fileInput) fileInput.value = '';
        }
      },
      error: (error) => {
        console.error('Error importing jobs:', error);
        this.isImporting = false;

        if (error.status === 400 && error.error?.errors) {
          this.importErrors = error.error.errors;
          this.toastr.error(`Import validation failed with ${this.importErrors.length} errors`, 'Import Failed');
        } else {
          this.importErrors = ['Failed to import jobs. Please check the file format and try again.'];
          this.toastr.error('Failed to process import', 'Server Error');
        }
      },
    });
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
    this.jobService.delete(id).subscribe({
      next: (data) => {
        if (data) {
          this.toastr.success('Delete success', 'Success')
          this.searchData();
        }
      },
      error: (error) => {
        this.toastr.error('Failed to delete jobs', 'Error');
        console.error('Error delete jobs:', error);
      },
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
