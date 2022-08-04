import { Component } from '@angular/core';
import {AuthenticationService} from "../../../Services/authentication.service";
import {Router} from "@angular/router";
import {storageItems} from "../../../model/storageItems";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  loggedIn = true;
  username: string | null = localStorage.getItem(storageItems.Username);

  constructor(private authSerive: AuthenticationService, private router: Router) {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    const role: string | undefined | null = localStorage.getItem(storageItems.Role);
    this.authSerive.logout();
    this.loggedIn = false;
    if( role === "admin"){
      this.router.navigate(['/home']);
    }
  }
}
