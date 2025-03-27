import { Observable} from "rxjs";
import { LevelModel } from "../../models/level/level.model";

export interface ILevelService{
    getAll():Observable<LevelModel[]>
}