export class TableColumn {
  public name!: string; // Is Active
  public value!: string; // isActive
  formatter?: (job: any) => string;
}
