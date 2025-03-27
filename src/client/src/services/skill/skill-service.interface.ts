import { Observable } from "rxjs";
import { SkillModel } from "../../models/skill/skill.modes";

export interface ISkillService {
    getAll():Observable<SkillModel[]>
}
