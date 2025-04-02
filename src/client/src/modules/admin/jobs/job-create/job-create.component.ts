import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { finalize } from 'rxjs';
import { BENEFIT_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../../../../constants/injection/injection.constant';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { BenefitModel } from '../../../../models/data-for-input/benefit.modes';
import { CommonModule } from '@angular/common';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';

@Component({
  selector: 'app-job-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './job-create.component.html',
  styleUrl: './job-create.component.css'
})
export class JobCreateComponent implements OnInit {
  public jobForm!: FormGroup;

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
    @Inject(LEVEL_SERVICE) private readonly levelService: ILevelService,
    @Inject(SKILL_SERVICE) private readonly skillService: ISkillService,
    @Inject(BENEFIT_SERVICE) private readonly benefitService: IBenefitService,
  ) { }

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

  public onSubmit(): void {
    if (this.jobForm.valid) {
      console.log('Form submitted:', this.jobForm.value);
      // Send data to backend
      // this.jobService.createJob(this.jobForm.value)
      //   .subscribe({
      //     next: (response) => {
      //       console.log('Job created successfully:', response);
      //       this.router.navigate(['/admin/jobs']);
      //     },
      //     error: (error) => {
      //       console.error('Error creating job:', error);
      //       // Handle error (show error message, etc.)
      //     }
      //   });
    } else {
      // Mark all fields as touched to trigger validation visuals
      Object.keys(this.jobForm.controls).forEach(key => {
        this.jobForm.get(key)?.markAsTouched();
      });
    }
  }

  public cancel(): void {
    this.router.navigate(['/admin/jobs']);
  }
}
