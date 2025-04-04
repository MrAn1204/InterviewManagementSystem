import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewTableComponent } from './table.component';

describe('TableComponent', () => {
  let component: InterviewTableComponent;
  let fixture: ComponentFixture<InterviewTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InterviewTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
