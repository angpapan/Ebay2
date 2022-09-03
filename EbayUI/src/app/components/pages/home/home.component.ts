import { Component, OnInit } from '@angular/core';
import {ItemBoxItem} from "../../../model/ItemBoxItem";
import {ItemSimple} from "../../../model/Items/ItemSimple";
import { ItemGridBlockComponent } from "../../shared/item-grid-block/item-grid-block.component";


import { ItemService} from "../../../Services/item.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  items: ItemBoxItem[];
  page:number=1;
  newItems: any = [];

  constructor(private itemService: ItemService) { }

  ngOnInit(): void {
    this.items = [
      new ItemBoxItem(135, "Item1", "Item1 description"),
      new ItemBoxItem(2, "Item2", "Item2 description"),
      new ItemBoxItem(3, "Item3", "Item3 description"),
      new ItemBoxItem(4, "Item4", "Item4 description"),
      new ItemBoxItem(5, "Item5", "Item5 description"),
      new ItemBoxItem(6, "Item6", "Item6 description"),
    ]

    this.itemService.getTester(this.page).subscribe(
      {next: response=>{
        //response.body?.forEach(i=>this.newItems.push(i));
          console.log(this.page);
        this.newItems = response.body;
        }
      }
    )
  }

  getPage(){
    this.itemService.getTester(this.page).subscribe(
      {next:response=>{
        console.log(response.headers.get('X-pagination'));
        console.log(response.body);
          //this.newItems = this.newItems.concat(response.body);
        }

      }
    )
  }
}
