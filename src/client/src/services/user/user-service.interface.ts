import { Observable } from "rxjs";
import { PaginatedResult } from "../../models/paginated-result.model";
import { User } from "../../models/user/user.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IUserService extends IMasterDataService<User> {
  getUsers(params: {
    search?: string;
    departmentId?: number;
    isActive?: boolean;
    roles?: string[];
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PaginatedResult<User>>;

  checkUnique(username: string | null, email: string | null): Observable<{ usernameExists: boolean, emailExists: boolean }>;

  inactiveUser(id: number): Observable<any>;
  activeUser(id: number): Observable<any>;
}
