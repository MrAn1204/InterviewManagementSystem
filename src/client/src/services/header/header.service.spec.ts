import { TestBed } from '@angular/core/testing';

import { HeaderService } from './header.service';

describe('HeaderService', () => {
  let service: HeaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HeaderService]
    });
    service = TestBed.inject(HeaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with default title "Homepage"', () => {
    service.title$.subscribe(title => {
      expect(title).toBe('Homepage');
    });
  });

  it('should update title when setTitle is called', () => {
    const newTitle = 'New Page Title';

    service.setTitle(newTitle);

    service.title$.subscribe(title => {
      expect(title).toBe(newTitle);
    });
  });

  it('should emit new title to subscribers', (done) => {
    const newTitle = 'Another Title';

    service.title$.subscribe(title => {
      if (title === newTitle) {
        expect(title).toBe(newTitle);
        done();
      }
    });

    service.setTitle(newTitle);
  });
});