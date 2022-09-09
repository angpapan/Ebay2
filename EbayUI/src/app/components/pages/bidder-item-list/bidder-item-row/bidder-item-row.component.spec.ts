import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidderItemRowComponent } from './bidder-item-row.component';

describe('BidderItemRowComponent', () => {
  let component: BidderItemRowComponent;
  let fixture: ComponentFixture<BidderItemRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BidderItemRowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BidderItemRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
