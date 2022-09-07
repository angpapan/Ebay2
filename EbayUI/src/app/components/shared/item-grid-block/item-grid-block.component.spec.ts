import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemGridBlockComponent } from './item-grid-block.component';

describe('ItemGridBlockComponent', () => {
  let component: ItemGridBlockComponent;
  let fixture: ComponentFixture<ItemGridBlockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemGridBlockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemGridBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
