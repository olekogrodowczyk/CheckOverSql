import { TestBed } from '@angular/core/testing';

import { SendQueryService } from './send-query.service';

describe('SendQueryService', () => {
  let service: SendQueryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SendQueryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
