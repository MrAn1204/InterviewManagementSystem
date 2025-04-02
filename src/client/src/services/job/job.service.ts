import { Injectable } from '@angular/core';
import { JobModel } from '../../models/job/job.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../models/paginated-result.model';
import { MasterDataService } from '../master-data/master-data.service';
import { IJobService } from './job-service.interface';

@Injectable({
  providedIn: 'root'
})
export class JobService extends MasterDataService<JobModel> implements IJobService{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Job');
  }
}
