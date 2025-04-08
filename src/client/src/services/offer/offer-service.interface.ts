import { Observable } from "rxjs";
import { OfferModel } from "../../models/offer/offer.model";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";

export interface IOffService {
  getAll(): Observable<PaginatedResult<OfferModel>>;

  search(filter: any): Observable<PaginatedResult<OfferModel>>;

  getById(id: number): Observable<OfferModel>;

  create(data: any): Observable<boolean>;

  update(id: number, data: any): Observable<boolean>;

  changeStatus(data: any): Observable<boolean>;

  exportOffers(data: any): Observable<Blob>;
}