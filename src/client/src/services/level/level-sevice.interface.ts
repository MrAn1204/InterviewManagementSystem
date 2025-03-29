import { Observable} from "rxjs";
import { LevelModel } from "../../models/data-for-input/level.model";

export interface ILevelService{
    getAll():Observable<LevelModel[]>
}