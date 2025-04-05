import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import { OFFER_SERVICE } from '../../../constants/injection/injection.constant';
import { IOffService } from '../../../services/offer/offer-service.interface';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-export-offer-modal',
  imports: [CommonModule, FormsModule ],
  templateUrl: './export-offer-modal.component.html',
  styleUrl: './export-offer-modal.component.css'
})
export class ExportOfferModalComponent {
  constructor(
    @Inject(OFFER_SERVICE) private offerService: IOffService,
  ) { }

  startDate: string = '';
  endDate: string = '';

  @Input() isOpen: boolean = false;
  @Output() closeModal = new EventEmitter<void>();

  close() {
    this.closeModal.emit();
  }

  exportOffer(){
    const date = {
      fromTo: this.startDate,
      endTo: this.endDate
    };

    console.log('Sending data:', date);

    this.offerService.exportOffers(date).subscribe({
      next: (response: Blob) => {
        // Tạo đối tượng URL cho Blob
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        
        // Tạo URL cho đối tượng Blob
        const url = window.URL.createObjectURL(blob);
        
        // Tạo một liên kết (anchor) ẩn
        const a = document.createElement('a');
        a.href = url;
        a.download = 'offers.xlsx'; // Đặt tên tệp tải xuống
        a.click(); // Tự động nhấp vào liên kết để tải tệp xuống
  
        // Giải phóng URL khi không còn cần thiết
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.log('Error exporting file:', error);
        console.error('Error exporting file:', error);
      }
    });

    this.close();
  }
}
