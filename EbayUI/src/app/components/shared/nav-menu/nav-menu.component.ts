import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from "../../../Services/authentication.service";
import {Router} from "@angular/router";
import {storageItems} from "../../../model/storageItems";
import {MessageService} from "../../../Services/message.service";
import {Category} from "../../../model/Category";
import {CategoryService} from "../../../Services/category.service";
import {Obj} from "@popperjs/core";
import {SwalService} from "../../../Services/swal.service";
import Swal from "sweetalert2";
import {AdminService} from "../../../Services/admin.service";

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
  enabled: string | null = localStorage.getItem(storageItems.Enabled);
  selectedCategory: number = 0;
  topCategories: Category[] = [];
  searchText: string = "";

  constructor(private authSerive: AuthenticationService, private router: Router,
              private messageService: MessageService, private categoryService: CategoryService,
              private swalService: SwalService, private adminService: AdminService) {}

  ngOnInit() {
    let token: string | null = localStorage.getItem(storageItems.Token);
    this.loggedIn = token !== undefined && token !== null && token !== "";

    this.categoryService.getTopCategories().subscribe({
      next: value => this.topCategories = value
    })

    if(this.loggedIn && this.enabled === "true"){
      this.getNewMessages();
    }


  }

  getNewMessages() {
    if(this.loggedIn && this.enabled === "true"){
      this.messageService.checkForNew().subscribe({
        next: value => {
          console.log(value);
          this.newMessages = value;
          setTimeout(() => {this.getNewMessages()}, 10000)
        }
      })
    }
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
    let query: object = {};
    if(this.searchText !== ''){
      query = {search: this.searchText};
    }
    if(Number(this.selectedCategory) !== 0){
      query = {...query, categories: this.selectedCategory};
    }

    console.log(query);


    this.router.navigate(['/search'], {queryParams: query});
  }

  factorize(): void {
    if(this.role !== "admin"){
      Swal.fire({
        ...this.swalService.BootstrapConfirmOnlyOptions,
        icon: 'error',
        title: 'Action not available!',
        html: 'Sorry, you are not an administrator'
      })
      return;
    }


    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon: 'question',
      title: 'Recalculate Recommendations?',
      html: `Are you sure you want to recalculate the recommendations? This action may take a lot of time.`
    }).then(result => {
      if(result.isConfirmed){
        this.adminService.factorize().subscribe({
          next: response => {
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              title: 'Success',
              html: `Recalculation was successful!`
            })
          }
        })
      }
    })




  }

}
