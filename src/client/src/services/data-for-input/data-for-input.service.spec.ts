import { TestBed } from '@angular/core/testing';

import { DataForInputService } from './data-for-input.service';

describe('DataForInputService', () => {
  let service: DataForInputService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataForInputService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
