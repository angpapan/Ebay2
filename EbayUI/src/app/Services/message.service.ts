import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserListRequest} from "../model/UserListRequest";
import {UserListItem} from "../model/UserListItem";
import {SendMessageRequest} from "../model/messages/SendMessageRequest";
import {InboxRequest} from "../model/messages/InboxRequest";
import {MessageDetailResponse} from "../model/messages/MessageDetailResponse";
import {MessageListResponse} from "../model/messages/MessageListResponse";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  private messageUrl = 'https://localhost:7088/message';

  sendMessage(req: SendMessageRequest): Observable<string> {
    return this.http.post<string>(this.messageUrl + '/send', req);
  }

  getInbox(req: InboxRequest): Observable<HttpResponse<MessageListResponse[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof InboxRequest]).join('&');
    console.log(queryString);
    const url = this.messageUrl + `/inbox?${queryString}`;

    return this.http.get<MessageListResponse[]>(url, {observe: "response"});
  }

  getOutbox(req: InboxRequest): Observable<HttpResponse<MessageListResponse[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof InboxRequest]).join('&');
    console.log(queryString);
    const url = this.messageUrl + `/outbox?${queryString}`;

    return this.http.get<MessageListResponse[]>(url, {observe: "response"});
  }

  getMessage(req: number): Observable<MessageDetailResponse> {
    return this.http.get<MessageDetailResponse>(`${this.messageUrl}/${req}`);
  }

  deleteMessage(req: number): Observable<HttpResponse<void>> {
    return this.http.delete<HttpResponse<void>>(`${this.messageUrl}/${req}`);
  }
}
