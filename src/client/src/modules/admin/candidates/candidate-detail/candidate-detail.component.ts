import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import {
  CANDIDATE_SERVICE,
  COMMON_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { AuthService } from '../../../../services/auth/auth.service';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ToastrService } from 'ngx-toastr';
import { CandidateModel } from '../../../../models/candidate/candidate.model';

@Component({
  selector: 'app-candidate-detail',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './candidate-detail.component.html',
  styleUrl: './candidate-detail.component.css',
})
export class CandidateDetailComponent {
  public isDropDownOpen = false;
  candidateId!: number;
  public candidate!: CandidateModel;
  public form!: FormGroup;
  public skillList: SkillModel[] = [];
  public levelList: LevelModel[] = [];
  public usersInput: UserForInputModel[] = [];
  public selectableCandidateStatuses: CandidateStatusModel[] = [];
  constructor(
    @Inject(CANDIDATE_SERVICE)
    private readonly candidateService: ICandidateService,
    @Inject(COMMON_SERVICE)
    private readonly commonService: ICommonService,
    private readonly route: ActivatedRoute,
    @Inject('IAuthService') private readonly authService: IAuthService,
    private readonly toastService: ToastrService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.commonService.getSkillData().subscribe((data) => {
      this.skillList = data;
    });
    this.commonService.getLevelData().subscribe((data) => {
      this.levelList = data;
    });
    this.commonService
      .getUserForInputData(['ADMIN', 'MANAGER', 'RECRUITER'])
      .subscribe((data) => {
        this.usersInput = data;
      });
    this.commonService
      .getSelectableCandidateStatus()
      .subscribe((data) => {
        this.selectableCandidateStatuses = data;
      });
    this.candidateId = +this.route.snapshot.paramMap.get('id')!;
    this.candidateService.getById(this.candidateId).subscribe((res) => {
      this.candidate = res;
    });
  }

  public getSkillNames(skillList: SkillModel[]): string {
    if (!skillList.length) return '';
    return skillList
      .map((skill) => {
        return skill ? skill.skillName : '';
      })
      .filter((name) => name)
      .join(', ');
  }
}
