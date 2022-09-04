import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from "../../../Services/authentication.service";
import {Router} from "@angular/router";
import {storageItems} from "../../../model/storageItems";
import {MessageService} from "../../../Services/message.service";
import {Category} from "../../../model/Category";
import {CategoryService} from "../../../Services/category.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  loggedIn: boolean;
  role: string | null = localStorage.getItem(storageItems.Role);
  newMessages: number = 0;
  username: string | null = localStorage.getItem(storageItems.Username);
  selectedCategory: number = 0;
  topCategories: Category[] = [];
  searchText: string = "";

  constructor(private authSerive: AuthenticationService, private router: Router,
              private messageService: MessageService, private categoryService: CategoryService) {}

  ngOnInit() {
    let token: string | null = localStorage.getItem(storageItems.Token);
    this.loggedIn = token !== undefined && token !== null && token !== "";

    this.categoryService.getTopCategories().subscribe({
      next: value => this.topCategories = value
    })

    if(this.role === 'user'){
      this.getNewMessages();
    }


  }

  getNewMessages() {
    this.messageService.checkForNew().subscribe({
      next: value => {
        console.log(value);
        this.newMessages = value;
        setTimeout(() => {this.getNewMessages()}, 10000)
      }
    })
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authSerive.logout();
    this.loggedIn = false;
    this.router.navigate(['/home']);
  }

  search(): void {
    console.log(`Text: ${this.searchText}, Category: ${this.selectedCategory}`);
    // TODO navigate to search results page giving above parameters as state
  }
}
