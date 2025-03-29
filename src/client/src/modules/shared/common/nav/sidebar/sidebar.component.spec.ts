import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarComponent } from './sidebar.component';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { SidebarService } from '../../../../../services/sidebar/sidebar.service';

describe('SidebarComponent', () => {
  let component: SidebarComponent;
  let fixture: ComponentFixture<SidebarComponent>;
  let sidebarService: SidebarService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        FontAwesomeModule, 
        CommonModule
      ],
      declarations: [SidebarComponent],
      providers: [SidebarService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SidebarComponent);
    component = fixture.componentInstance;
    sidebarService = TestBed.inject(SidebarService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have correct FontAwesome icons defined', () => {
    expect(component.faHome).toBeTruthy();
    expect(component.faGear).toBeTruthy();
    expect(component.faBriefcase).toBeTruthy();
    expect(component.faFileAlt).toBeTruthy();
    expect(component.faUser).toBeTruthy();
    expect(component.faUsers).toBeTruthy();
    expect(component.faCalendarAlt).toBeTruthy();
    expect(component.faAngleDoubleLeft).toBeTruthy();
    expect(component.faAngleDoubleRight).toBeTruthy();
  });

  describe('Resize Behavior', () => {
    it('should switch to mobile mode when window width is less than 768px', () => {
      spyOn(sidebarService, 'switchMobile');
      spyOn(sidebarService, 'toggleSidebar');

      // Mock window width less than 768px
      Object.defineProperty(window, 'innerWidth', { value: 767 });
      
      // Simulate resize event
      const resizeEvent = new Event('resize');
      window.dispatchEvent(resizeEvent);

      // Verify service methods were called
      expect(sidebarService.switchMobile).toHaveBeenCalled();
    });

    it('should switch back to desktop mode when window width is 768px or more', () => {
      // First set mobile mode
      sidebarService.switchMobile();

      spyOn(sidebarService, 'switchMobile');
      spyOn(sidebarService, 'toggleSidebar');

      // Mock window width 768px or more
      Object.defineProperty(window, 'innerWidth', { value: 768 });
      
      // Simulate resize event
      const resizeEvent = new Event('resize');
      window.dispatchEvent(resizeEvent);

      // Verify service methods were called
      expect(sidebarService.switchMobile).toHaveBeenCalled();
    });
  });

  it('should call toggleSidebar on sidebar service', () => {
    spyOn(sidebarService, 'toggleSidebar');
    
    component.toggleSidebar();
    
    expect(sidebarService.toggleSidebar).toHaveBeenCalled();
  });

  it('should call switchMobile on sidebar service', () => {
    spyOn(sidebarService, 'switchMobile');
    
    component.switchMobile();
    
    expect(sidebarService.switchMobile).toHaveBeenCalled();
  });
});