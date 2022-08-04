import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsLineComponent } from './user-details-line.component';

describe('UserDetailsLineComponent', () => {
  let component: UserDetailsLineComponent;
  let fixture: ComponentFixture<UserDetailsLineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserDetailsLineComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserDetailsLineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
