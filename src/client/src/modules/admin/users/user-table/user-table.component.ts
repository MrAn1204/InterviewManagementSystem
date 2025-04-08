import { Component  } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TableComponent } from '../../../../core/components/table/table.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-table',
  imports: [FontAwesomeModule,FormsModule, CommonModule],
  templateUrl: './user-table.component.html',
  styleUrl: './user-table.component.css'
})
export class UserTableComponent  extends TableComponent {

  constructor (private readonly router: Router) {
    super();
  }
  view(id: number): void {

    this.router.navigate([`/admin/users/${id}/detail`])
  }

  public edit(id: string): void {
    this.router.navigate([`/admin/users/${id}/edit`])
  }
}

