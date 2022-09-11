import { AfterViewInit, Component, Input } from '@angular/core';
import {SellerItem} from "../../../../model/Items/SellerItem";


@Component({
  selector: '[app-seller-all-items-row]',
  templateUrl: './seller-all-items-row.component.html',
  styleUrls: ['./seller-all-items-row.component.css']
})

export class SellerAllItemsRowComponent implements AfterViewInit {
  @Input() item: SellerItem;
  visible: boolean = true;

  constructor() { }

  ngAfterViewInit(): void {
    let source: any;

    if(this.item.image !== undefined && this.item.image !== null){
      const byteArray = new Uint8Array(atob(this.item.image!).split('').map(char => char.charCodeAt(0)));
      console.log(byteArray);
      let bl: Blob = new Blob([byteArray]);
      source = window.URL.createObjectURL(bl);
    }
    else {
      source = "../../../../../assets/itemBoxItemDefault.jpg";
    }

    let postPreviewImage = <HTMLInputElement>document.getElementById(`item-thumbnail-${this.item.itemId}`);
    postPreviewImage.src = source;
  }



}

