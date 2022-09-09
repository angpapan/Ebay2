import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidderItemListComponent } from './bidder-item-list.component';

describe('BidderItemListComponent', () => {
  let component: BidderItemListComponent;
  let fixture: ComponentFixture<BidderItemListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BidderItemListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BidderItemListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
