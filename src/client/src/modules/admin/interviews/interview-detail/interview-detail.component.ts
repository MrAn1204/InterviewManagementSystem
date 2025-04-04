import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { INTERVIEW_SERVICE } from '../../../../constants/injection/injection.constant';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { EventEmitter } from 'stream';

@Component({
  selector: 'app-interview-detail',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './interview-detail.component.html',
  styleUrl: './interview-detail.component.css'
})
export class InterviewDetailComponent implements OnInit {
  public interview!: InterviewModel;
  public id!: number;

  constructor(
    @Inject(INTERVIEW_SERVICE) private readonly interviewService: IInterviewService,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.id = Number(params.get('id'));
      this.interviewService.getById(this.id).subscribe((res) => this.interview = res);
    });
  }
}
