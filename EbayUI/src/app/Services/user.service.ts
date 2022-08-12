import { Injectable } from '@angular/core';
import { User } from '../model/user';
import { Observable } from 'rxjs';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {UserDetails} from "../model/UserDetails";
import {AuthenticationResponse} from "../model/AuthenticationResponse";
import {Login} from "../model/login";
import {UserRegisterRequest} from "../model/UserRegisterRequest";

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json', Accept: 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl = 'https://localhost:7088/user';

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl);
  }

  getUser(username: string): Observable<UserDetails> {
    const url = this.usersUrl + '/' + username;
    return this.http.get<UserDetails>(url);
  }

  registerUser(reg: UserRegisterRequest): Observable<HttpResponse<AuthenticationResponse>> {
    return this.http.post<AuthenticationResponse>(this.usersUrl + '/register', reg, { observe: 'response'});
  }

  checkUsernameExistence(username: string): Observable<boolean> {
    const url = this.usersUrl + '/exists/' + username;
    return this.http.get<boolean>(url);
  }

  constructor(private http: HttpClient) { }
}
