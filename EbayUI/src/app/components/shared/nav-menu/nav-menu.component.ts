import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from "../../../Services/authentication.service";
import {Router} from "@angular/router";
import {storageItems} from "../../../model/storageItems";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  loggedIn: boolean;
  username: string | null = localStorage.getItem(storageItems.Username);

  constructor(private authSerive: AuthenticationService, private router: Router) {}

  ngOnInit() {
    let token: string | null = localStorage.getItem(storageItems.Token);
    this.loggedIn = token !== undefined && token !== null && token !== "";
  }

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
    this.router.navigate(['/home']);
  }
}
