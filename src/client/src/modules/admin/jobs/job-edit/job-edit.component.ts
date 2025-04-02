import { Component, Inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BENEFIT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../../../../constants/injection/injection.constant';
import { BenefitModel } from '../../../../models/data-for-input/benefit.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { JobModel } from '../../../../models/job/job.model';
import { HeaderService } from '../../../../services/header/header.service';
import { IJobService } from '../../../../services/job/job-service.interface';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-job-edit',
  imports: [RouterLink, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './job-edit.component.html',
  styleUrl: './job-edit.component.css'
})
export class JobEditComponent {
  public data!: JobModel;
  public jobForm!: FormGroup;

  public skillsDropdownOpen = false;
  public benefitsDropdownOpen = false;
  public levelDropdownOpen = false;
  public skills: SkillModel[] = [];
  public levels: LevelModel[] = [];
  public benefits: BenefitModel[] = [];

  constructor(
    private readonly headerService: HeaderService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    @Inject(LEVEL_SERVICE) private readonly levelService: ILevelService,
    @Inject(SKILL_SERVICE) private readonly skillService: ISkillService,
    @Inject(BENEFIT_SERVICE) private readonly benefitService: IBenefitService,
  ) {

  }

  public ngOnInit(): void {
    this.initForm();
    this.loadData();
    
    this.headerService.setTitle('Job Detail');
    this.getJobDetail();
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
      jobTitle: ['', Validators.required],
      skills: [''],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      salaryFrom: [''],
      salaryTo: [''],
      workingAddress: [''],
      benefits: ['', Validators.required],
      level: ['', Validators.required],
      description: ['']
    });
  }

  private getJobDetail(): void {
    const jobId = Number(this.route.snapshot.paramMap.get('id'));
    if (jobId) {
      this.jobService.getById(jobId).subscribe({
        next: (response) => {
          this.data = response;
        },
        error: (error) => {
          console.error('Error fetching job detail:', error);
        }
      });
    } else {
      console.error('No job ID provided in route parameters.');
    }
  }

  private loadData(): void {
    // Load skills
    this.skillService.getAll()      
      .subscribe({
        next: (data) => {
          this.skills = data.map(skill => ({
            ...skill,
            selected: false
          }));
        },
        error: (error) => {
          console.error('Error loading skills:', error);
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
      case 'level':
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
      case 'level':
        this.levels[index].selected = !this.levels[index].selected;
        this.updateFormControl('level');
        break;
    }
  }

  public updateFormControl(type: string): void {
    let selectedIds: number[] = [];
    let selectedValues: string = '';

    switch (type) {
      case 'skills':
        selectedIds = this.skills
          .filter(item => item.selected)
          .map(item => item.id);
        selectedValues = this.skills
          .filter(item => item.selected)
          .map(item => item.id)
          .join(',');
        break;
      case 'level':
        selectedIds = this.levels
          .filter(item => item.selected)
          .map(item => item.id);
        selectedValues = this.levels
          .filter(item => item.selected)
          .map(item => item.id)
          .join(',');
        break;
      case 'benefits':
        selectedValues = this.benefits
          .filter(item => item.selected)
          .map(item => item.benefitName)
          .join(',');
        break;
    }

    this.jobForm.get(type)?.setValue(selectedValues);
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
      case 'level':
        return this.levels
          .filter(item => item.selected)
          .map(item => item.levelName)
          .join(', ');
      default:
        return '';
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
