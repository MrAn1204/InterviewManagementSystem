import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  imports: [CommonModule],
  templateUrl: './confirm-modal.component.html',
  styleUrl: './confirm-modal.component.css'
})
export class ConfirmModalComponent {
  @Input() isOpen: boolean = false; 
  @Output() onClose: EventEmitter<void> = new EventEmitter<void>();
  @Output() onConfirm: EventEmitter<void> = new EventEmitter<void>(); 

  public closeModal() {
    this.onClose.emit(); 
  }

  public confirmDelete() {
    this.onConfirm.emit(); 
    this.closeModal();
  }
}
