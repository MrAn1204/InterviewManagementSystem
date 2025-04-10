import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  imports: [CommonModule],
  templateUrl: './confirm-modal.component.html',
  styleUrl: './confirm-modal.component.css',
})
export class ConfirmModalComponent {
  @Input() isOpen: boolean = false;
  @Input() message: string = '';
  @Output() onClose: EventEmitter<void> = new EventEmitter<void>();
  @Output() onConfirm: EventEmitter<void> = new EventEmitter<void>();

  public closeModal() {
    this.onClose.emit();
  }

  public confirm() {
    this.onConfirm.emit();
    this.closeModal();
  }
  public getMessage() {
    return this.message === ''
      ? 'Are you sure you want to delete this item?'
      : this.message;
  }
}
