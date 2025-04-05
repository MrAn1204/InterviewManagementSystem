export class SearchModel {
  public keyword!: string;
  public departmentName?: string;
  public status!: string;
  public pageNumber!: number;
  public pageSize!: number;
  public orderBy!: string;
  public orderDirection!: OrderDirection;
}

export enum OrderDirection {
  ASC = 0,
  DESC = 1,
}
