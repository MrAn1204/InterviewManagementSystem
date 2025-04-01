import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SkillModel } from '../../../../models/data-for-input/skill.modes';
import { LevelModel } from '../../../../models/data-for-input/level.model';
import { UserForInputModel } from '../../../../models/data-for-input/user-for-input.model';
import { CandidateStatusModel } from '../../../../models/candidate/candidate-status.model';
import {
  CANDIDATE_SERVICE,
  DATA_FOR_INPUT_SERVICE,
} from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { IDataForInputService } from '../../../../services/data-for-input/data-for-input-service.interface';

@Component({
  selector: 'app-candidate-detail',
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './candidate-detail.component.html',
  styleUrl: './candidate-detail.component.css',
})
export class CandidateDetailComponent {
  public isDropDownOpen = false;
  candidateId!: number;
  public form!: FormGroup;
  public skillList: SkillModel[] = [];
  public levelList: LevelModel[] = [];
  public usersInput: UserForInputModel[] = [];
  public selectableCandidateStatuses: CandidateStatusModel[] = [];
  public cvFilePath!: string;
  public oldFilePath: string = '';
  constructor(
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(DATA_FOR_INPUT_SERVICE)
    private dataForInputService: IDataForInputService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.createForm();
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
    this.candidateId = +this.route.snapshot.paramMap.get('id')!;
    this.candidateService.getById(this.candidateId).subscribe((res) => {
      this.form.patchValue({
        fullName: res.fullName,
        email: res.email,
        dob: this.convertDateFormat(res.dateOfBirth || ''),
        address: res.address,
        phoneNumber: res.phoneNumber,
        gender: res.gender,
        note: res.note,
        position: res.currentPosition,
        status: res.status,
        recruiter: res.recruiter.id,
        experience: res.experience,
        highestLevel: res.highestLevel?.id,
      });
      console.log(this.form.value.dob);

      this.cvFilePath = res.cv || '';

      // Xử lý riêng FormArray skills
      const skillsArray = this.form.get('skills') as FormArray;
      skillsArray.clear(); // Xóa các skill cũ (nếu có)
      res.candidateSkills?.forEach((cs) => {
        skillsArray.push(new FormControl(cs.id));
      });
    });
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
      recruiter: new FormControl(0, Validators.required),
      experience: new FormControl(0),
      highestLevel: new FormControl(0, Validators.required),
    });
  }

  onSubmit() {
    console.log(this.form.value);
    this.candidateService
      .update(
        this.candidateId.toString(),
        this.form.value,
        this.form.value.cvAttachment,
        this.oldFilePath
      )
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
    this.oldFilePath = this.cvFilePath;
    this.cvFilePath = '';
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.form.patchValue({ cvAttachment: fileInput.files[0] });
    }
  }

  removeFile() {
    this.oldFilePath = this.cvFilePath;
    this.cvFilePath = '';
    this.form.patchValue({ cvAttachment: null });
  }

  getFileName(filePath: string): string {
    return filePath.split('/').pop() || 'CV File';
  }

  convertDateFormat(dateString: string): string {
    if (!dateString) return '';

    const [year, month, day] = dateString.split('T')[0].split('-');
    return `${year}-${month}-${day}`;
  }
}
