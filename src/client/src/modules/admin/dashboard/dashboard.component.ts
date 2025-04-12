import { Component, Inject, OnInit } from '@angular/core';
import { HeaderService } from '../../../services/header/header.service';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { BarChartComponent } from '../../charts/bar-chart/bar-chart.component';
import {
  CANDIDATE_SERVICE,
  JOB_SERVICE,
} from '../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../services/candidate/candidate-service.interface';
import { CandidateCountByWeekModel } from '../../../models/candidate/candidate-count-by-week.model';
import { IJobService } from '../../../services/job/job-service.interface';
import { JobCountByWeekModel } from '../../../models/job/job-count-by-week.model';

@Component({
  selector: 'app-dashboard',

  imports: [BarChartComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  candidateChartData: CandidateCountByWeekModel[] = [];
  candidateChartRealData: number[] = [];
  jobChartData: JobCountByWeekModel[] = [];
  jobChartRealData: number[] = [];

  constructor(
    public sidebarService: SidebarService,
    public headerService: HeaderService,
    @Inject(CANDIDATE_SERVICE)
    private readonly candidateService: ICandidateService,
    @Inject(JOB_SERVICE)
    private readonly jobService: IJobService
  ) {}

  ngOnInit(): void {
    this.loadCandidateDataChart();
    this.loadJobDataChart();
  }

  loadCandidateDataChart(
    month: number = new Date().getMonth() + 1,
    year: number = new Date().getFullYear()
  ) {
    this.candidateService
      .getCandidateCountByWeek(year, month)
      .subscribe((res) => {
        this.candidateChartData = res;
        if (this.candidateChartData != null) {
          this.candidateChartRealData = this.candidateChartData.map(
            (o) => o.candidateCount
          );
        }
      });
  }

  loadJobDataChart(
    month: number = new Date().getMonth() + 1,
    year: number = new Date().getFullYear()
  ) {
    this.jobService.getJobCountByWeek(year, month).subscribe((res) => {
      this.jobChartData = res;
      if (this.jobChartData != null) {
        this.jobChartRealData = this.jobChartData.map((o) => o.jobCount);
      }
    });
  }

  onMonthYearCandidateSearchChanged(event: { month: number; year: number }) {
    this.loadCandidateDataChart(event.month, event.year);
  }

  onMonthYearJobSearchChanged(event: { month: number; year: number }) {
    this.loadJobDataChart(event.month, event.year);
  }
}
