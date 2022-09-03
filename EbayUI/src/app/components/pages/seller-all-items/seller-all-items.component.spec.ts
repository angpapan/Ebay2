import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerAllItemsComponent } from './seller-all-items.component';

describe('SellerAllItemsComponent', () => {
  let component: SellerAllItemsComponent;
  let fixture: ComponentFixture<SellerAllItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerAllItemsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerAllItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
