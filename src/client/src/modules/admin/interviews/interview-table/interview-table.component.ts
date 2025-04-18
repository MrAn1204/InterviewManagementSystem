import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterModule } from '@angular/router';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FormsModule } from '@angular/forms';
import { formatTime } from "../../../../helpers/format-schedule.helper";

@Component({
  selector: 'app-interview-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule, RouterModule],
  templateUrl: './interview-table.component.html',
  styleUrl: './interview-table.component.css'
})
export class InterviewTableComponent extends TableComponent {
  constructor () {
    super();
  }

  public mapTime(time: string): string {
    return formatTime(time);
  }
}
