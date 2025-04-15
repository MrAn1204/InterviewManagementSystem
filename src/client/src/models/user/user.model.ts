// user.model.ts
export class User {
  id!: number;
  username!: string;
  email!: string;
  fullName!: string;
  address?: string;
  phoneNumber? : string;
  dob?: string;
  isActive!: boolean;
  createdDate?: string;
  gender? :string;
  updatedDate?: string;
  departmentName?: string;
  roles!: string[];
  note ?: string
}

