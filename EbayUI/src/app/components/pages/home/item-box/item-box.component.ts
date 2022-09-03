import {Component, Input, OnInit} from '@angular/core';
import {ItemBoxItem} from "../../../../model/ItemBoxItem";
import {ItemSimple} from "../../../../model/Items/ItemSimple";

@Component({
  selector: 'app-item-box',
  templateUrl: './item-box.component.html',
  styleUrls: ['./item-box.component.css']
})
export class ItemBoxComponent implements OnInit {
  @Input() items: ItemBoxItem[];
  @Input() newItems: ItemSimple[];
  @Input() title: string;

  constructor() { }

  ngOnInit(): void {

  }

}
