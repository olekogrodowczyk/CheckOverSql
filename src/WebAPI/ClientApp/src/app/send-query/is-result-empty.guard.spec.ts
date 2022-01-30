import { TestBed } from '@angular/core/testing';

import { IsResultEmptyGuard } from './is-result-empty.guard';

describe('IsResultEmptyGuard', () => {
  let guard: IsResultEmptyGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(IsResultEmptyGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
