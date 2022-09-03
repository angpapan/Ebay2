import { Component, OnInit } from '@angular/core';
import {ItemBoxItem} from "../../../model/ItemBoxItem";
import {storageItems} from "../../../model/storageItems";
import {ItemService} from "../../../Services/item.service";
import {forkJoin, Observable} from "rxjs";
import { zipAll } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  loading: boolean = true;
  recommendedItems: ItemBoxItem[];
  hotItems: ItemBoxItem[];
  newItems: ItemBoxItem[];
  isLoggedIn: boolean;

  constructor(private itemService: ItemService) { }

  ngOnInit(): void {
    let token: string | null = localStorage.getItem(storageItems.Token);
    this.isLoggedIn = token !== undefined && token !== null && token !== "";

    this.loading = true;

    let obsvArray = [this.itemService.getHotItems(), this.itemService.getNewItems()];
    if(this.isLoggedIn){
      obsvArray = [...obsvArray, this.itemService.getRecommendedItems()];
    }
    const observable = forkJoin(obsvArray);
    observable.subscribe({
      next: value => {
        this.hotItems = value[0];
        this.newItems = value[1];
        this.recommendedItems = value[2];
        this.loading = false;
      }
    });
  }

}
