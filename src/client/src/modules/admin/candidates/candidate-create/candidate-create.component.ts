import { Component, Inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import {
  AUTH_SERVICE,
  CANDIDATE_SERVICE,
  COMMON_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { CommonModule } from '@angular/common';
import { ICommonService } from '../../../../services/data-for-input/common-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-candidate-create',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './candidate-create.component.html',
  styleUrl: './candidate-create.component.css',
})
export class CandidateCreateComponent implements OnInit {
  public isDropDownOpen = false;

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
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    private readonly toastService: ToastrService,
    private readonly route: Router
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
    this.createForm();
  }

  public createForm() {
    this.form = new FormGroup({
      fullName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      dob: new FormControl(''),
      address: new FormControl(''),
      phoneNumber: new FormControl(''),
      gender: new FormControl('', Validators.required),
      cvAttachment: new FormControl(null),
      note: new FormControl(''),
      position: new FormControl('', Validators.required),
      skills: new FormArray([], Validators.required),
      status: new FormControl('', Validators.required),
      recruiter: new FormControl('', Validators.required),
      experience: new FormControl(0),
      highestLevel: new FormControl(0, Validators.min(1)),
    });
  }

  onSubmit() {
    console.log(this.form.value);
    this.candidateService
      .create(this.form.value, this.form.value.cvAttachment)
      .subscribe({
        next: (res) => {
          this.toastService.success('Add candidate successfully!', 'success');
          this.route.navigate(['/admin/candidates']);
        },
        error: () => {
          this.toastService.error('Add candidate unsuccessfully!', 'error');
        },
      });
  }

  // Hàm kiểm tra skill đã được chọn chưa
  isSkillSelected(skillId: number): boolean {
    return (this.form.get('skills') as FormArray).controls.some(
      (control) => control.value === skillId
    );
  }

  // Hàm cập nhật danh sách kỹ năng đã chọn
  updateSkills(event: any, skillId: number) {
    const skillsFormArray = this.form.get('skills') as FormArray;

    if (event.target.checked) {
      // Thêm vào danh sách nếu chưa có
      skillsFormArray.push(new FormControl(skillId));
    } else {
      // Bỏ chọn nếu đã có
      const index = skillsFormArray.controls.findIndex(
        (control) => control.value === skillId
      );
      if (index !== -1) {
        skillsFormArray.removeAt(index);
      }
    }
  }

  // Lấy danh sách skill đã chọn để hiển thị trong input
  getSelectedSkills(): string {
    return this.skillList
      .filter((skill) => this.isSkillSelected(skill.id))
      .map((skill) => skill.skillName)
      .join(', ');
  }

  removeSkill(skillId: number) {
    const skillsFormArray = this.form.get('skills') as FormArray;
    const index = skillsFormArray.controls.findIndex(
      (control) => control.value === skillId
    );
    if (index !== -1) {
      skillsFormArray.removeAt(index);
    }
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.form.patchValue({ cvAttachment: fileInput.files[0] });
    }
  }

  removeFile() {
    this.form.patchValue({ cvAttachment: null });
  }

  assignMe(): void {
    this.authService.getUserInformation().subscribe((res) => {
      this.form.patchValue({ recruiter: res?.id });
    });
  }
}
