import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemSmallBoxComponent } from './item-small-box.component';

describe('ItemSmallBoxComponent', () => {
  let component: ItemSmallBoxComponent;
  let fixture: ComponentFixture<ItemSmallBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemSmallBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemSmallBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
