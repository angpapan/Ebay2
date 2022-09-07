import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemFullViewComponent } from './item-full-view.component';

describe('ItemFullViewComponent', () => {
  let component: ItemFullViewComponent;
  let fixture: ComponentFixture<ItemFullViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemFullViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemFullViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
