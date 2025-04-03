export interface Department {
  id: number;
  departmentName: string;
}

// Hoặc nếu cần class
export class Department {
  constructor(
    public id: number,
    public departmentName: string
  ) {}
}
