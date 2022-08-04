import { Component, OnInit } from '@angular/core';
import {ItemBoxItem} from "../../../model/ItemBoxItem";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  items: ItemBoxItem[];

  constructor() { }

  ngOnInit(): void {
    this.items = [
      new ItemBoxItem(1, "Item1", "Item1 description"),
      new ItemBoxItem(2, "Item2", "Item2 description"),
      new ItemBoxItem(3, "Item3", "Item3 description"),
      new ItemBoxItem(4, "Item4", "Item4 description"),
      new ItemBoxItem(5, "Item5", "Item5 description"),
      new ItemBoxItem(6, "Item6", "Item6 description"),
    ]
  }

}
