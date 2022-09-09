import {AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {SellerItem} from "../../../../model/Items/SellerItem";
import {ItemService} from "../../../../Services/item.service";
import {SwalService} from "../../../../Services/swal.service";
import {BidderItem} from "../../../../model/Items/BidderItem";
import {SendToState} from "../../../../model/messages/SendToState";

@Component({
  selector: '[app-bidder-item-row]',
  templateUrl: './bidder-item-row.component.html',
  styleUrls: ['./bidder-item-row.component.css']
})
export class BidderItemRowComponent implements AfterViewInit, OnInit {
  @Input() item: BidderItem;
  visible: boolean = true;
  currentDate: Date = new Date();
  endDate: Date;
  sendTo: SendToState;

  constructor(private itemService: ItemService, private swalService: SwalService) {
  }

  ngOnInit(): void {
    this.endDate = new Date(this.item.ends);

    this.sendTo = new SendToState();
    this.sendTo.usernameTo = this.item.sellerUsername;
    this.sendTo.messageSubject = `About: ${this.item.name}`;
  }

  ngAfterViewInit(): void {
    let source: any;

    if (this.item.image !== undefined && this.item.image !== null) {
      const byteArray = new Uint8Array(atob(this.item.image!).split('').map(char => char.charCodeAt(0)));
      console.log(byteArray);
      let bl: Blob = new Blob([byteArray]);
      source = window.URL.createObjectURL(bl);
    } else {
      source = "../../../../../assets/no-image-available.jpg";
    }

    let postPreviewImage = <HTMLInputElement>document.getElementById(`item-thumbnail-${this.item.itemId}`);
    postPreviewImage.src = source;
  }
}
