import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {Category} from "../model/Category";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  private categoryUrl = 'https://localhost:7088/category';
  private locationUrl = 'https://localhost:7088/locations';

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.categoryUrl);
  }

  getLocations(): Observable<string[]>{
    return this.http.get<string[]>(this.locationUrl);
  }

  getTopCategories(num?: number | undefined): Observable<Category[]> {
    return this.http.get<Category[]>(this.categoryUrl + `/top${num !== undefined ? `?num=${num}` : ''}`);

  }
}
