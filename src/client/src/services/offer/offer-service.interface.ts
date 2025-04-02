import { Observable } from "rxjs";
import { OfferModel } from "../../models/offer/offer.model";
import { PaginatedResult } from "../../models/candidate/paginated-result.model";

export interface IOffService {
  getAll(): Observable<OfferModel[]>;

  search(filter: any): Observable<PaginatedResult<OfferModel>>;

  getById(id: number): Observable<OfferModel>;

  create(candidate: any, cvAttachment: File): Observable<boolean>;

  delete(id: string): Observable<boolean>;
}