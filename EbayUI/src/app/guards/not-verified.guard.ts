import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import { Observable } from 'rxjs';
import {storageItems} from "../model/storageItems";

@Injectable({
  providedIn: 'root'
})
export class NotVerifiedGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (localStorage.getItem(storageItems.Token)) {
      // user is already logged in

      if(localStorage.getItem(storageItems.Enabled) == "false"){
        // not enabled
        return true;
      }

      // already enabled
      this.router.navigate(['/home']);
      return false;
    }

    this.router.navigate(['/login']);
    return  false;
  }

}
