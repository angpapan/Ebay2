import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TryUploadComponent } from './try-upload.component';

describe('TryUploadComponent', () => {
  let component: TryUploadComponent;
  let fixture: ComponentFixture<TryUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TryUploadComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TryUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
