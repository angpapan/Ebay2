import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {SellerListRequest} from "../model/Items/SellerListRequest";
import {SellerItem} from "../model/Items/SellerItem";
import {UserListRequest} from "../model/UserListRequest";
import {UserListItem} from "../model/UserListItem";
import {EditItemInfoResponse} from "../model/Items/EditItemInfoResponse";
import {ItemBoxItem} from "../model/ItemBoxItem";

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(private http: HttpClient) { }

  private itemUrl = 'https://localhost:7088/item';

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
