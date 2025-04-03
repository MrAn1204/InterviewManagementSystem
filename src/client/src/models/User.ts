// user.model.ts
export interface User {
  id: number;
  userName: string;
  email: string;
  fullName: string;
  address?: string;
  phoneNumber : string;
  dob?: string; // Đổi thành string nếu API trả về dạng ISO
  isActive: boolean; // Thay đổi từ 'status' sang 'isActive'
  createdDate: string;
  gender:string;
  updatedDate?: string;
  departmentName?: string;
  roles: string[];
  note ?: string
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
