import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CANDIDATE_SERVICE, LEVEL_SERVICE, SKILL_SERVICE } from '../../../../constants/injection/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { ILevelService } from '../../../../services/level/level-sevice.interface';
import { SkillModel } from '../../../../models/skill/skill.modes';
import { LevelModel } from '../../../../models/level/level.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-candidate-create',
  imports: [RouterLink,CommonModule,ReactiveFormsModule],
  templateUrl: './candidate-create.component.html',
  styleUrl: './candidate-create.component.css',
})
export class CandidateCreateComponent implements OnInit {
  public isDropDownOpen=false;

  public form!: FormGroup;
  public skillList:SkillModel[]=[];
  public levelList:LevelModel[]=[];
  constructor(
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(SKILL_SERVICE) private skillService:ISkillService,
    @Inject(LEVEL_SERVICE) private levelService:ILevelService
  ) {}

  ngOnInit(): void {
    this.createForm;
    this.skillService.getAll().subscribe((data)=>{
      this.skillList=data;
    })
    this.levelService.getAll().subscribe((data)=>{
      this.levelList=data;
    })
    this.createForm()
  }

  // public toggleSkillDropDown():void{

  // }

  public createForm() {
    this.form=new FormGroup({
      fullName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      dob: new FormControl(null),
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
      highestLevel: new FormControl(0, Validators.required)
    });
  }

  onSubmit(){

  }
}
