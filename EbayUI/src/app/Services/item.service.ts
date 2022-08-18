import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(private http: HttpClient) { }

  private itemUrl = 'https://localhost:7088/item';

  createItem(req: FormData): Observable<string> {
    return this.http.post<string>(this.itemUrl, req);
  }
}
