import { Injectable } from '@angular/core';
import {UserRegisterRequest} from "../model/UserRegisterRequest";
import {Observable} from "rxjs";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {AuthenticationResponse} from "../model/AuthenticationResponse";
import {UserDetails} from "../model/UserDetails";
import {UserListRequest} from "../model/UserListRequest";
import {UserListItem} from "../model/UserListItem";
import {query} from "@angular/animations";

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  private adminUrl = 'https://localhost:7088/admin';

  verifyUser(user: string): Observable<HttpResponse<string>> {
    return this.http.post<string>(this.adminUrl + '/confirmUser/?username=' + user, {}, { observe: 'response'});
  }

  getAllUsers(req: UserListRequest): Observable<HttpResponse<UserListItem[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof UserListRequest]).join('&');
    console.log(queryString);
    const url = this.adminUrl + `/users?${queryString}`;
    return this.http.get<UserListItem[]>(url, {observe: "response"});
  }

  exportData(type: string, items: number[]): Observable<string>{
    let item_ids = '';
    for(const item of items){
      item_ids += `&item_ids=${item}`;
    }

    const requestOptions: Object = {
      responseType: 'text'
    }

    return this.http.get<string>(this.adminUrl + `/extract?type=${type}${item_ids}`, requestOptions);
  }

  factorize(): Observable<void> {
    const url = this.adminUrl + `/factorize`;
    return this.http.get<void>(url);
  }
}
