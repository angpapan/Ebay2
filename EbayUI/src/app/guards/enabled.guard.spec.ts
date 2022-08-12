import { TestBed } from '@angular/core/testing';

import { EnabledGuard } from './enabled.guard';

describe('EnabledGuard', () => {
  let guard: EnabledGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(EnabledGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
