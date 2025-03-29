export class PaginatedResult<T> {
    public pageNumber!: number;
    public pageSize!: number;
    public totalCount!: number;
    public totalPages!: number;
    public items!: T[];
}