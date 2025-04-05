import { LevelModel } from '../data-for-input/level.model';
import { SkillModel } from '../data-for-input/skill.modes';
import { UserForInputModel } from '../data-for-input/user-for-input.model';

export class CandidateModel {
  id!: number;
  fullName!: string;
  email!: string;
  phoneNumber!: string;
  address!: string;
  gender?: number | null;
  dateOfBirth?: string | null;
  currentPosition?: string | null;
  note?: string | null;
  experience!: number;
  cv?: string | null;
  status!: string;
  candidateSkills?: SkillModel[] | null;
  recruiter!: UserForInputModel;
  highestLevel!: LevelModel;
  createdDate?: string;
  updatedDate?: string;
}
