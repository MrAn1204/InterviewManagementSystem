import { Component, Inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  CANDIDATE_SERVICE,
  DATA_FOR_INPUT_SERVICE,
  LEVEL_SERVICE,
  SKILL_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { CommonModule } from '@angular/common';
import { IDataForInputService } from '../../../../services/data-for-input/data-for-input-service.interface';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';

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
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(DATA_FOR_INPUT_SERVICE)
    private dataForInputService: IDataForInputService
  ) {}

  ngOnInit(): void {
    this.createForm;
    this.dataForInputService.getSkillData().subscribe((data) => {
      this.skillList = data;
    });
    this.dataForInputService.getLevelData().subscribe((data) => {
      this.levelList = data;
    });
    this.dataForInputService
      .getUserForInputData(['ADMIN', 'MANAGER', 'RECRUITER'])
      .subscribe((data) => {
        this.usersInput = data;
      });
    this.dataForInputService
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
      position: new FormControl(''),
      skills: new FormArray([], Validators.required),
      status: new FormControl('', Validators.required),
      recruiter: new FormControl('', Validators.required),
      experience: new FormControl(0),
      highestLevel: new FormControl(0, Validators.required),
    });
  }

  onSubmit() {
    console.log(this.form.value);
    this.candidateService
      .create(this.form.value, this.form.value.cvAttachment)
      .subscribe((data) => {
        console.log(data);
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
}
