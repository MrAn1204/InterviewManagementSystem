import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ToastrService } from 'ngx-toastr';
import { of, BehaviorSubject } from 'rxjs';
import { UserInformation } from '../../../../../models/auth/user-information.model';
import { HeaderService } from '../../../../../services/header/header.service';
import { SidebarService } from '../../../../../services/sidebar/sidebar.service';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let mockAuthService: any;
  let mockSidebarService: any;
  let mockHeaderService: any;
  let mockRouter: any;
  let mockToastrService: any;
  let mockUserInfo: UserInformation;

  beforeEach(async () => {
    // Mock user information
    mockUserInfo = {
      id: '1',
      email: 'test@example.com',
      displayName: 'Test User',
      username: 'testuser',
      roles: ['User']
    };

    // Create mock services
    mockAuthService = {
      getUserInformation: jasmine.createSpy('getUserInformation').and.returnValue(of(mockUserInfo)),
      logout: jasmine.createSpy('logout')
    };

    mockSidebarService = {
      toggleSidebar: jasmine.createSpy('toggleSidebar'),
      isCollapsed$: new BehaviorSubject<boolean>(false),
      isMobile$: new BehaviorSubject<boolean>(false)
    };

    mockHeaderService = {
      title$: new BehaviorSubject<string>('Test Title')
    };

    mockRouter = {
      navigate: jasmine.createSpy('navigate')
    };

    mockToastrService = {
      warning: jasmine.createSpy('warning')
    };

    await TestBed.configureTestingModule({
      imports: [CommonModule, FontAwesomeModule],
      providers: [
        { provide: 'IAuthService', useValue: mockAuthService },
        { provide: SidebarService, useValue: mockSidebarService },
        { provide: HeaderService, useValue: mockHeaderService },
        { provide: Router, useValue: mockRouter },
        { provide: ToastrService, useValue: mockToastrService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load user information on init', () => {
    expect(mockAuthService.getUserInformation).toHaveBeenCalled();
    expect(component.userInfo).toEqual(mockUserInfo);
  });

  it('should toggle profile dropdown when method is called', () => {
    const mockEvent = jasmine.createSpyObj('Event', ['stopPropagation']);
    component.isShowProfileDropdown = false;

    component.toggleProfileDropdown(mockEvent);

    expect(mockEvent.stopPropagation).toHaveBeenCalled();
    expect(component.isShowProfileDropdown).toBe(true);

    component.toggleProfileDropdown(mockEvent);
    expect(component.isShowProfileDropdown).toBe(false);
  });

  it('should handle logout correctly', () => {
    component.onLogout();

    expect(mockAuthService.logout).toHaveBeenCalled();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
    expect(mockToastrService.warning).toHaveBeenCalledWith('You were Logout');
  });

  it('should close dropdown when clicking outside', () => {
    component.isShowProfileDropdown = true;
    component.profileMenuElement = document.createElement('div');
    spyOn(component.profileMenuElement, 'contains').and.returnValue(false);

    const event = new MouseEvent('click', { bubbles: true });
    document.dispatchEvent(event);

    expect(component.isShowProfileDropdown).toBeFalse();
  });


  it('should not close dropdown when clicking inside profileMenuElement', () => {
    // Setup
    component.isShowProfileDropdown = true;
    component.profileMenuElement = document.createElement('div');

    // Mock for contains method
    spyOn(component.profileMenuElement, 'contains').and.returnValue(true);

    // Call method with a target that is inside profileMenuElement
    component.clickOutside(new MouseEvent('click'));

    // Verify dropdown stays open
    expect(component.isShowProfileDropdown).toBeTrue();
  });

  it('should toggle sidebar when sidebar button is clicked', () => {
    const button = fixture.nativeElement.querySelector('button[title="Toggle menu"]');
    button.click();

    expect(mockSidebarService.toggleSidebar).toHaveBeenCalled();
  });

  it('should display the correct title from HeaderService', () => {
    mockHeaderService.title$.next('Updated Title');
    fixture.detectChanges();

    const titleElement = fixture.nativeElement.querySelector('h1');
    expect(titleElement.textContent.trim()).toBe('Updated Title');
  });

  it('should display user information correctly', () => {
    fixture.detectChanges();

    const displayNameElement = fixture.nativeElement.querySelector('.profile-menu p:first-child');
    const rolesElement = fixture.nativeElement.querySelector('.profile-menu p:last-child');

    expect(displayNameElement.textContent.trim()).toEqual(mockUserInfo.displayName);
    expect(rolesElement.textContent.trim()).toEqual(mockUserInfo.roles.join(', '));
  });
});
