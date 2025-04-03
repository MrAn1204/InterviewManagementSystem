import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  imports: [CommonModule],
  templateUrl: './confirm-modal.component.html',
  styleUrl: './confirm-modal.component.css'
})
export class ConfirmModalComponent {
  @Input() isOpen: boolean = false; // Controls modal visibility
  @Output() onClose = new EventEmitter<void>(); // Emits when modal is closed
  @Output() onConfirm = new EventEmitter<void>(); // Emits when delete is confirmed

  closeModal() {
    this.onClose.emit(); // Notify parent to close the modal
  }

  confirmDelete() {
    this.onConfirm.emit(); // Notify parent to proceed with deletion
    this.closeModal(); // Optionally close the modal after confirming
  }
}
