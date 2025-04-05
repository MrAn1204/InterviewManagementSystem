import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BENEFIT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../../../../constants/injection/injection.constant';
import { BenefitModel } from '../../../../models/data-for-input/benefit.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { JobModel } from '../../../../models/job/job.model';
import { HeaderService } from '../../../../services/header/header.service';
import { IJobService } from '../../../../services/job/job-service.interface';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { forkJoin } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-job-edit',
  imports: [RouterLink, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './job-edit.component.html',
  styleUrl: './job-edit.component.css'
})
export class JobEditComponent implements OnInit {
  public data!: JobModel;
  public jobForm!: FormGroup;

  public skillsDropdownOpen = false;
  public benefitsDropdownOpen = false;
  public levelDropdownOpen = false;
  public skills: SkillModel[] = [];
  public levels: LevelModel[] = [];
  public benefits: BenefitModel[] = [];
  public isLoading = true;

  constructor(
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    @Inject(LEVEL_SERVICE) private readonly levelService: ILevelService,
    @Inject(SKILL_SERVICE) private readonly skillService: ISkillService,
    @Inject(BENEFIT_SERVICE) private readonly benefitService: IBenefitService,
  ) {}

  public ngOnInit(): void {    
    this.createEmptyForm();
    this.loadAllData();
    
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

  private createEmptyForm(): void {
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
      status: new FormControl(''),
      description: new FormControl('')
    });
  }
  
  private loadAllData(): void {
    const jobId = Number(this.route.snapshot.paramMap.get('id'));
    if (!jobId) {
      console.error('No job ID provided in route parameters.');
      this.isLoading = false;
      return;
    }
    
    forkJoin({
      job: this.jobService.getById(jobId),
      skills: this.skillService.getAll(),
      levels: this.levelService.getAll(),
      benefits: this.benefitService.getAll()
    }).subscribe({
      next: (results) => {
        this.data = results.job;
        
        this.skills = results.skills.map(skill => ({
          ...skill,
          selected: this.data.skills.some(s => s.id === skill.id)
        }));
        
        this.levels = results.levels.map(level => ({
          ...level,
          selected: this.data.levels.some(l => l.id === level.id)
        }));
        
        this.benefits = results.benefits.map(benefit => ({
          ...benefit,
          selected: this.data.benefits.some(b => b.id === benefit.id)
        }));
        
        this.initForm();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading data:', error);
        this.isLoading = false;
      }
    });
  }

  private initForm(): void {
    if (this.data) {
      this.jobForm.patchValue({
        title: this.data.title,
        startDate: this.data.startDate?.toString().substring(0, 10) ?? '',
        endDate: this.data.endDate?.toString().substring(0, 10) ?? '',
        salaryMin: this.data.salaryMin,
        salaryMax: this.data.salaryMax,
        workingAddress: this.data.workingAddress,
        status: this.data.status,
        description: this.data.description
      });
      
      this.updateFormControl('skills');
      this.updateFormControl('benefits');
      this.updateFormControl('levels');
    }
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
      console.log('Form submitted:', this.jobForm.value);

      const jobId = Number(this.route.snapshot.paramMap.get('id'));
      if (jobId) {
        this.data = { ...this.data, ...this.jobForm.value };        
        console.log('Updated data:', this.data);
        
        this.jobService.update(jobId, this.data).subscribe({
          next: (response) => {
            this.toastr.success('Update successful!', 'Success');
            this.router.navigate(['/admin/jobs']);
          },
          error: (err) => {
            this.toastr.error('Update Error!', 'Error');
          },
        });
      }
    } else {
      Object.keys(this.jobForm.controls).forEach(key => {
        this.jobForm.get(key)?.markAsTouched();
      });
    }
  }

  public getBenefitNames(benefits: BenefitModel[]): string {
    return this.jobService.getBenefitNames(benefits);
  }

  public getSkillNames(skills: SkillModel[]): string {
    return this.jobService.getSkillNames(skills);
  }

  public getLevelNames(levels: LevelModel[]): string {
    return this.jobService.getLevelNames(levels);
  }
}