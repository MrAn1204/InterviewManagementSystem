// user.model.ts
export interface User {
  id: number;
  username: string;
  email: string;
  fullName: string;
  address?: string;
  phoneNumber : string;
  dob?: string;
  isActive: boolean;
  createdDate: string;
  gender? :string;
  updatedDate?: string;
  departmentName?: string;
  roles: string[];
  note ?: string
}

// export interface PaginatedResult<T> {
//   items: T[];
//   totalCount: number;
//   pageNumber: number;
//   pageSize: number;
//   totalPages: number;
// }
