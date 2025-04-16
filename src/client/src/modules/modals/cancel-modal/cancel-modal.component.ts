import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-cancel-modal',
  imports: [CommonModule],
  templateUrl: './cancel-modal.component.html',
  styleUrl: './cancel-modal.component.css'
})
export class CancelModalComponent {
  @Input() isOpen: boolean = false;

  @Input() action: string = '';

  @Input() entity: string = '';

  @Output() onClose = new EventEmitter<void>(); 

  @Output() onConfirm = new EventEmitter<void>(); 

  public closePopup(): void {
    this.onClose.emit();
  }

  public confirmCancel(): void {
    this.onConfirm.emit();
  }
}
