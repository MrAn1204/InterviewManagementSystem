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

  getRoleClass(role: string): string {
    switch (role) {
      case 'ADMIN':
        return 'bg-red-100 border-red-300 text-red-700';
      case 'RECRUITER':
        return 'bg-blue-100 border-blue-300 text-blue-700';
      case 'INTERVIEWER':
        return 'bg-green-100 border-green-300 text-green-700';
      case 'MANAGER':
        return 'bg-yellow-100 border-yellow-300 text-yellow-700';
      default:
        return 'bg-gray-100 border-gray-300 text-gray-700';
    }
  }

}

