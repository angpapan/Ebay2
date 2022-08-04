import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {storageItems} from "../model/storageItems";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (localStorage.getItem(storageItems.Token)) {
      console.log(1);
      if(localStorage.getItem(storageItems.Enabled) === "false"){
        console.log(2);
        this.router.navigate(['/not-verified']);
        return false;
      }
      else {
        // logged in so return true
        return true;
      }
    }
    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login']);
    return false;
  }

}
