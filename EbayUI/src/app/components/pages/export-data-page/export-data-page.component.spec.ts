import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportDataPageComponent } from './export-data-page.component';

describe('ExportDataPageComponent', () => {
  let component: ExportDataPageComponent;
  let fixture: ComponentFixture<ExportDataPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportDataPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExportDataPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
