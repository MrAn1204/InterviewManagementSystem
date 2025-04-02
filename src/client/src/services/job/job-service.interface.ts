import { BenefitModel } from "../../models/data-for-input/benefit.modes";
import { LevelModel } from "../../models/data-for-input/level.model";
import { SkillModel } from "../../models/data-for-input/skill.modes";
import { JobModel } from "../../models/job/job.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IJobService extends IMasterDataService<JobModel> {
    getBenefitNames(benefits: BenefitModel[]): string;

    getSkillNames(skills: SkillModel[]): string;

    getLevelNames(levels: LevelModel[]): string;
}
