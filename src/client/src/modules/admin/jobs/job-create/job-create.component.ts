import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { finalize } from 'rxjs';
import { AUTH_SERVICE, BENEFIT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../../../../constants/injection/injection.constant';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { BenefitModel } from '../../../../models/data-for-input/benefit.modes';
import { CommonModule } from '@angular/common';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';
import { IJobService } from '../../../../services/job/job-service.interface';
import { ToastrService } from 'ngx-toastr';
import { JobModel } from '../../../../models/job/job.model';
import { IAuthService } from '../../../../services/auth/auth-service.interface';

@Component({
  selector: 'app-job-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './job-create.component.html',
  styleUrl: './job-create.component.css'
})
export class JobCreateComponent implements OnInit {
  public jobForm!: FormGroup;
  public data!: JobModel;

  public skillsDropdownOpen = false;
  public benefitsDropdownOpen = false;
  public levelDropdownOpen = false;
  public skills: SkillModel[] = [];
  public levels: LevelModel[] = [];
  public benefits: BenefitModel[] = [];

  public loading = false;
  public loadingError = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly router: Router,
    private readonly toast: ToastrService,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    @Inject(LEVEL_SERVICE) private readonly levelService: ILevelService,
    @Inject(SKILL_SERVICE) private readonly skillService: ISkillService,
    @Inject(BENEFIT_SERVICE) private readonly benefitService: IBenefitService,
  ) { 

  }

  public ngOnInit(): void {
    this.initForm();
    this.loadData();
    // Close dropdowns when clicking outside
    document.addEventListener('click', (event: Event) => {
      const target = event.target as HTMLElement;
      if (!target.closest('#skills-wrapper')) {
        this.skillsDropdownOpen = false;
      }
      if (!target.closest('#benefits-wrapper')) {
        this.benefitsDropdownOpen = false;
      }
      if (!target.closest('#level-wrapper')) {
        this.levelDropdownOpen = false;
      }
    });
  }

  private initForm(): void {
    this.jobForm = this.fb.group({
      title: new FormControl('', Validators.required),
      skills: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      endDate: new FormControl('', Validators.required),
      salaryMin: new FormControl(null),
      salaryMax: new FormControl(null),
      workingAddress: new FormControl(''),
      benefits: new FormControl('', Validators.required),
      levels: new FormControl('', Validators.required),
      description: new FormControl('')
    });
    this.jobForm.get('startDate')?.setValue(new Date().toISOString().substring(0, 10));
    this.jobForm.get('endDate')?.setValue(new Date().toISOString().substring(0, 10));
  }

  private loadData(): void {
    this.loading = true;
    this.loadingError = false;

    // Load skills
    this.skillService.getAll()
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe({
        next: (data) => {
          this.skills = data.map(skill => ({
            ...skill,
            selected: false
          }));
        },
        error: (error) => {
          console.error('Error loading skills:', error);
          this.loadingError = true;
        }
      });

    // Load levels
    this.levelService.getAll()
      .subscribe({
        next: (data) => {
          this.levels = data.map(level => ({
            ...level,
            selected: false
          }));
        },
        error: (error) => {
          console.error('Error loading levels:', error);
          this.loadingError = true;
        }
      });

    // Load benefits
    this.benefitService.getAll()
      .subscribe({
        next: (data) => {
          this.benefits = data.map(benefit => ({
            ...benefit,
            selected: false
          }));
        },
        error: (error) => {
          console.error('Error loading benefits:', error);
          this.loadingError = true;
        }
      });
  }

  public toggleDropdown(dropdown: string, event: Event): void {
    event.stopPropagation();

    switch (dropdown) {
      case 'skills':
        this.skillsDropdownOpen = !this.skillsDropdownOpen;
        this.benefitsDropdownOpen = false;
        this.levelDropdownOpen = false;
        break;
      case 'benefits':
        this.benefitsDropdownOpen = !this.benefitsDropdownOpen;
        this.skillsDropdownOpen = false;
        this.levelDropdownOpen = false;
        break;
      case 'levels':
        this.levelDropdownOpen = !this.levelDropdownOpen;
        this.skillsDropdownOpen = false;
        this.benefitsDropdownOpen = false;
        break;
    }
  }

  public updateSelection(type: string, index: number): void {
    switch (type) {
      case 'skills':
        this.skills[index].selected = !this.skills[index].selected;
        this.updateFormControl('skills');
        break;
      case 'benefits':
        this.benefits[index].selected = !this.benefits[index].selected;
        this.updateFormControl('benefits');
        break;
      case 'levels':
        this.levels[index].selected = !this.levels[index].selected;
        this.updateFormControl('levels');
        break;
    }
  }

  public updateFormControl(type: string): void {
    switch (type) {
      case 'skills': {
        const selectedSkillIds = this.skills
          .filter(item => item.selected)
          .map(item => item.id)
        this.jobForm.get('skills')?.setValue(selectedSkillIds);
        break;
      }
      case 'levels': {
        const selectedLevelIds = this.levels
          .filter(item => item.selected)
          .map(item => item.id)
        this.jobForm.get('levels')?.setValue(selectedLevelIds);
        break;
      }
      case 'benefits': {
        const selectedBenefitIds = this.benefits
          .filter(item => item.selected)
          .map(item => item.id)
        this.jobForm.get('benefits')?.setValue(selectedBenefitIds);
        break;
      }
    }
  }

  public getSelectedItemsText(type: string): string {
    switch (type) {
      case 'skills':
        return this.skills
          .filter(item => item.selected)
          .map(item => item.skillName)
          .join(', ');
      case 'benefits':
        return this.benefits
          .filter(item => item.selected)
          .map(item => item.benefitName)
          .join(', ');
      case 'levels':
        return this.levels
          .filter(item => item.selected)
          .map(item => item.levelName)
          .join(', ');
      default:
        return '';
    }
  }

  public onSubmit(): void {
    if (this.jobForm.valid) {
      this.data = this.jobForm.value;
      this.authService.getUserInformation().subscribe((user) => {
        this.data.createdBy = user?.id ? Number(user.id) : 1;
      });
      console.log('Form submitted:', this.data);
      
      this.jobService.create(this.data)
        .subscribe({
          next: (response) => {
            this.toast.success('Job created successfully!');
            this.router.navigate(['/admin/jobs']);
          },
          error: (error) => {
            this.toast.error('Error creating job!');
            console.error('Error creating job:', error);
          }
        });
    } else {
      Object.keys(this.jobForm.controls).forEach(key => {
        this.jobForm.get(key)?.markAsTouched();
      });
    }
  }

  public cancel(): void {
    this.router.navigate(['/admin/jobs']);
  }
}
