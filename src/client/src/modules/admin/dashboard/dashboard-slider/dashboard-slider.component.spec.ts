import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardSliderComponent } from './dashboard-slider.component';

describe('DashboardSliderComponent', () => {
  let component: DashboardSliderComponent;
  let fixture: ComponentFixture<DashboardSliderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardSliderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
