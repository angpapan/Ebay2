import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {UserListRequest} from "../model/UserListRequest";
import {BidderListRequest} from "../model/Items/BidderListRequest";
import {BidderItem} from "../model/Items/BidderItem";

@Injectable({
  providedIn: 'root'
})
export class BidService {

  constructor(private http: HttpClient) { }

  private itemUrl = 'https://localhost:7088/bids';

  getBidderItems(req: BidderListRequest): Observable<HttpResponse<BidderItem[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof UserListRequest]).join('&');
    console.log(queryString);
    const url = this.itemUrl + `/bidder-list?${queryString}`;
    return this.http.get<BidderItem[]>(url, {observe: "response"});
  }
}
