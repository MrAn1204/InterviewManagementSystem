import { Observable } from "rxjs";
import { BenefitModel } from "../../models/data-for-input/benefit.modes";

export interface IBenefitService {
    getAll(): Observable<BenefitModel[]>
}
