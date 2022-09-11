import { Component, AfterViewInit, Input } from '@angular/core';

import { ItemSimple } from "../../../model/Items/ItemSimple";

@Component({
  selector: 'app-item-small-box',
  templateUrl: './item-small-box.component.html',
  styleUrls: ['./item-small-box.component.css']
})
export class ItemSmallBoxComponent implements AfterViewInit {
  // uuid helps in case same item display twice in one page
  // otherwise postPreviewImage put images only in first appearance
  @Input() uuid: string;
  @Input() item: ItemSimple;

  constructor() {
  }

  ngAfterViewInit(): void {
    let source: string;
    if (this.item.image !== undefined && this.item.image !== null) {
      const byteArray = new Uint8Array(atob(this.item.image!).split('').map(char => char.charCodeAt(0)));
      let bl: Blob = new Blob([byteArray]);
      source = window.URL.createObjectURL(bl);
    } else {
      source = '../../../assets/no-image-available.jpg';
    }
    let postPreviewImage = <HTMLInputElement>document.getElementById(`item-image-${this.uuid}-${this.item.itemId}`);
    postPreviewImage.src = source;

  }

}
