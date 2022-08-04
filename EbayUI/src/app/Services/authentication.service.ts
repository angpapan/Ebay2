import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Login } from '../model/login';
import { AuthenticationResponse } from '../model/AuthenticationResponse';
import { Observable } from 'rxjs';
import {storageItems} from "../model/storageItems";

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<HttpResponse<AuthenticationResponse>> {
    const ln: Login = { username, password };
    return this.http.post<AuthenticationResponse>('https://localhost:7088/user/login', ln, { observe: 'response'});
  }

  logout(): void {
    // remove token from local storage to log user out
    localStorage.removeItem(storageItems.Token);
    localStorage.removeItem(storageItems.Enabled);
    localStorage.removeItem(storageItems.Username);
    localStorage.removeItem(storageItems.Role);
  }
}
