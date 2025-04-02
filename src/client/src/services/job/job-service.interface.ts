import { JobModel } from "../../models/job/job.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IJobService extends IMasterDataService<JobModel> {
}
