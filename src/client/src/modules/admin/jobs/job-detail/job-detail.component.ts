import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { JobModel } from '../../../../models/job/job.model';
import { JOB_SERVICE } from '../../../../constants/injection/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BenefitModel } from '../../../../models/data-for-input/benefit.modes';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-job-detail',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css'
})
export class JobDetailComponent implements OnInit {
  public data!: JobModel;

  constructor(
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    public readonly authService : AuthService,
    private readonly route: ActivatedRoute,
  ) {
  }

  public ngOnInit(): void {
    this.getJobDetail();
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
