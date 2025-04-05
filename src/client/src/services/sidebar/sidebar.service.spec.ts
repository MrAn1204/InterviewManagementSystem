import { TestBed } from '@angular/core/testing';

import { SidebarService } from './sidebar.service';

describe('SidebarService', () => {
  let service: SidebarService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SidebarService]
    });
    service = TestBed.inject(SidebarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with sidebar not collapsed', () => {
    expect(service.isCollapsed).toBeFalse();
  });

  it('should initialize with mobile mode off', () => {
    expect(service.isMobile).toBeFalse();
  });

  it('should toggle sidebar state', () => {
    // Initial state is false
    expect(service.isCollapsed).toBeFalse();

    // First toggle
    service.toggleSidebar();
    expect(service.isCollapsed).toBeTrue();

    // Second toggle
    service.toggleSidebar();
    expect(service.isCollapsed).toBeFalse();
  });

  it('should switch mobile state', () => {
    // Initial state is false
    expect(service.isMobile).toBeFalse();

    // First switch
    service.switchMobile();
    expect(service.isMobile).toBeTrue();

    // Second switch
    service.switchMobile();
    expect(service.isMobile).toBeFalse();
  });

  it('should emit sidebar collapsed state changes', (done) => {
    service.isCollapsed$.subscribe((isCollapsed) => {
      expect(isCollapsed).toBeFalse();
      done();
    });
  });

  it('should emit mobile state changes', (done) => {
    service.isMobile$.subscribe((isMobile) => {
      expect(isMobile).toBeFalse();
      done();
    });
  });
});
