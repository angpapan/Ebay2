import {Component, Input, OnInit} from '@angular/core';
import {ItemBoxItem} from "../../../../model/ItemBoxItem";
import {ItemSimple} from "../../../../model/Items/ItemSimple";
import {DomSanitizer} from "@angular/platform-browser";


@Component({
  selector: 'app-item-box',
  templateUrl: './item-box.component.html',
  styleUrls: ['./item-box.component.css']
})
export class ItemBoxComponent implements OnInit {
  @Input() items: ItemBoxItem[];
  @Input() newItems: ItemSimple[];
  @Input() title: string;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.items.forEach(it => {
      if(it.image !== null){
        const byteArray = new Uint8Array(atob(it.image!).split('').map(char => char.charCodeAt(0)));
        let bl: Blob = new Blob([byteArray]);
        it.image = this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(bl)) as string;
      }
      return it;
    })
  }

}
