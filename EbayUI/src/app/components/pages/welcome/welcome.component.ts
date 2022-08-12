import { Component, OnInit } from '@angular/core';
import {storageItems} from "../../../model/storageItems";

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

  isExpanded: boolean = false;
  loggedIn: boolean;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit() {
    let token: string | null = localStorage.getItem(storageItems.Token);
    this.loggedIn = token !== undefined && token !== null && token !== "";
  }

}
