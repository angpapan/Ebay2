import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerAllItemsRowComponent } from './seller-all-items-row.component';

describe('SellerAllItemsRowComponent', () => {
  let component: SellerAllItemsRowComponent;
  let fixture: ComponentFixture<SellerAllItemsRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerAllItemsRowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerAllItemsRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
