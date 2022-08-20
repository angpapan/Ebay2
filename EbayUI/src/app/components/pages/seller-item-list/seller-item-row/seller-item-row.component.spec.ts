import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerItemRowComponent } from './seller-item-row.component';

describe('SellerItemRowComponent', () => {
  let component: SellerItemRowComponent;
  let fixture: ComponentFixture<SellerItemRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerItemRowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerItemRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
