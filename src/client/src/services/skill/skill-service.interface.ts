import { Observable } from "rxjs";
import { SkillModel } from "../../models/data-for-input/skill.modes";

export interface ISkillService {
    getAll():Observable<SkillModel[]>
}
