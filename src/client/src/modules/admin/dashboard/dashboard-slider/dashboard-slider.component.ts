import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faBriefcase, faCalendarDay, faHandshake, faUsers, faUserTie } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../../../../services/user/user.service';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { InterviewService } from '../../../../services/interview/interview.service';
import { JobService } from '../../../../services/job/job.service';
import { OfferService } from '../../../../services/offer/offer.service';
import { AUTH_SERVICE, CANDIDATE_SERVICE, INTERVIEW_SERVICE, JOB_SERVICE, OFFER_SERVICE } from '../../../../constants/injection/injection.constant';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-dashboard-slider',
imports: [CommonModule, FontAwesomeModule, RouterModule],
  templateUrl: './dashboard-slider.component.html',
  styleUrl: './dashboard-slider.component.css'
})
export class DashboardSliderComponent implements OnInit {
  public faUsers = faUsers;
  public faUserTie = faUserTie;
  public faCalendarDay = faCalendarDay;
  public faHandshake = faHandshake;
  public faBriefcase = faBriefcase;

  public items! : any[];

  public users = 0;
  public candidates = 0;
  public interviews = 0;
  public jobs = 0;
  public offers = 0;

  constructor(
    private readonly userService: UserService,
    @Inject(CANDIDATE_SERVICE) private readonly candidateService: CandidateService,
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: InterviewService,
    @Inject(JOB_SERVICE) private readonly jobService: JobService,
    @Inject(OFFER_SERVICE) private readonly offerService: OfferService,
    @Inject(AUTH_SERVICE) public readonly authService: AuthService
  ) {
    inject(UserService);
  }

  ngOnInit(): void {
    this.userService.getUsers({}).subscribe((res) => {
      this.users = res.items.length;
    })
    this.candidateService.getAll().subscribe((res) => {
      this.candidates = res.length;
    });
    this.interviewService.getAll().subscribe((res) => {
      this.interviews = res.length;
    });
    this.jobService.getAll().subscribe((res) => {
      this.jobs = res.length;
    });
    this.offerService.getAll().subscribe((res) => {
      this.offers = res.items.length;
    });
  }
}
