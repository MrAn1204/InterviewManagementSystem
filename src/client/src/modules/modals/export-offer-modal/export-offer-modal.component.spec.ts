import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportOfferModalComponent } from './export-offer-modal.component';

describe('ExportOfferModalComponent', () => {
  let component: ExportOfferModalComponent;
  let fixture: ComponentFixture<ExportOfferModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExportOfferModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExportOfferModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
