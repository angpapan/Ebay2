import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerItemListComponent } from './seller-item-list.component';

describe('SellerItemListComponent', () => {
  let component: SellerItemListComponent;
  let fixture: ComponentFixture<SellerItemListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerItemListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerItemListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
