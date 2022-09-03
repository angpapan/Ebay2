import { Component, OnInit, Input } from '@angular/core';
import {ItemSimple} from "../../../model/Items/ItemSimple";
import { ItemBoxComponent } from "../../pages/home/item-box/item-box.component";

@Component({
  selector: 'app-item-grid-block',
  templateUrl: './item-grid-block.component.html',
  styleUrls: ['./item-grid-block.component.css']
})
export class ItemGridBlockComponent implements OnInit {
  @Input() items : ItemSimple[] ;

  constructor() { }

  ngOnInit(): void {
  }

}
