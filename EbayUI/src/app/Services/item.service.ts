import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {SellerListRequest} from "../model/Items/SellerListRequest";
import {SellerItem} from "../model/Items/SellerItem";
import {UserListRequest} from "../model/UserListRequest";
import {UserListItem} from "../model/UserListItem";
import {EditItemInfoResponse} from "../model/Items/EditItemInfoResponse";

import {ItemDetails} from "../model/Items/ItemDetailed";
import {ItemSimple} from "../model/Items/ItemSimple";
import {ItemDetailsFull} from "../model/Items/ItemDetailedFull";
import {ItemListRequest} from "../model/Items/ItemListRequest";

import {ItemBoxItem} from "../model/ItemBoxItem";


@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(private http: HttpClient) { }

  private itemUrl = 'https://localhost:7088/item';
  private url = 'https://localhost:7088';

  createItem(req: FormData): Observable<string> {
    return this.http.post<string>(this.itemUrl, req);
  }

  getEditInfo(id: number): Observable<EditItemInfoResponse> {
    return this.http.get<EditItemInfoResponse>(`${this.itemUrl}/edit-info/${id}`);
  }

  editAuction(id: number, req: FormData): Observable<string> {
    return this.http.put<string>(`${this.itemUrl}/${id}`, req);
  }

  startAuction(id: number): Observable<string> {
    return this.http.put<string>(`${this.itemUrl}/${id}/start`, {});
  }

  deleteAuction(id: number): Observable<string> {
    return this.http.delete<string>(`${this.itemUrl}/${id}`, {});
  }

  deleteImage(item_id: number, image_id: number): Observable<string> {
    return this.http.delete<string>(`${this.itemUrl}/${item_id}/image/${image_id}`, {});
  }

  getSellerItems(req: SellerListRequest): Observable<HttpResponse<SellerItem[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof UserListRequest]).join('&');
    console.log(queryString);
    const url = this.itemUrl + `/sells?${queryString}`;
    return this.http.get<SellerItem[]>(url, {observe: "response"});
  }


  getItemsBySeller(req: SellerListRequest, username: string):Observable<HttpResponse<SellerItem[]>>{
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof UserListRequest]).join('&');
    console.log("query service" + queryString);
    const url = this.itemUrl + `/user/${username}?${queryString}`;
    return this.http.get<SellerItem[]>(url, {observe: "response"});

  }

  getItemDetails(_itemId: number, preview: boolean): Observable<ItemDetails>{

    if(preview)
      return this.http.get<ItemDetails>(`${this.itemUrl}/${_itemId}/preview`, {});
    return this.http.get<ItemDetails>(`${this.itemUrl}/${_itemId}`, {});
  }

  getItemFullDetails(_itemId: number): Observable<ItemDetailsFull>{
    return this.http.get<ItemDetailsFull>(`${this.itemUrl}/full/${_itemId}`, {});
  }

  getTester(pn:number) : Observable<HttpResponse<ItemSimple[]>>{
    console.log("Service started");
    return this.http.get<ItemSimple[]>
    (`${this.itemUrl}/search?Categories=1&Categories=2&Categories=3&Categories=4&Categories=5&PageSize=5&PageNumber=${pn}`,{observe:"response"});
  }
  placebid(itemId:number , amount:number): Observable<string>{
    return this.http.post<string>(`${this.url}/createBid`,{"itemId":itemId,"amount":amount});
  }

  getSearcItemList(req : ItemListRequest) : Observable<HttpResponse<ItemSimple[]>>{
    let queryString = $.param(req).replaceAll(`%5D`,'').replaceAll(`%5B`,'');
    console.log(queryString);
    return this.http.get<ItemSimple[]>(`${this.itemUrl}/search?${queryString}`,{observe:"response"});
  }


  getRecommendedItems(num?: number | undefined): Observable<ItemBoxItem[]> {
    return this.http.get<ItemBoxItem[]>(`${this.itemUrl}/recommended${num !== undefined ? `?num=${num}` : ''}`);
  }

  getHotItems(num?: number | undefined): Observable<ItemBoxItem[]> {
    return this.http.get<ItemBoxItem[]>(`${this.itemUrl}/hot${num !== undefined ? `?num=${num}` : ''}`);
  }

  getNewItems(num?: number | undefined): Observable<ItemBoxItem[]> {
    return this.http.get<ItemBoxItem[]>(`${this.itemUrl}/new${num !== undefined ? `?num=${num}` : ''}`);
  }

}
