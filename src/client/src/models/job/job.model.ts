import { BenefitModel } from "../data-for-input/benefit.modes";
import { LevelModel } from "../data-for-input/level.model";
import { SkillModel } from "../data-for-input/skill.modes";

export class JobModel {
  id!: number;
  title!: string;
  workingAddress!: string;
  salaryMin!: number;
  salaryMax!: number;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  status!: string;
  createdBy!: number;
  skills!: SkillModel[];
  levels!: LevelModel[];
  benefits!: BenefitModel[];
  createdDate!: Date;
  updatedDate?: Date;
}
