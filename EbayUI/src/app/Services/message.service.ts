import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {SendMessageRequest} from "../model/messages/SendMessageRequest";
import {InboxRequest} from "../model/messages/InboxRequest";
import {MessageDetailResponse} from "../model/messages/MessageDetailResponse";
import {MessageListResponse} from "../model/messages/MessageListResponse";
import {OutboxRequest} from "../model/messages/OutboxRequest";
import {MessagesStats} from "../model/messages/MessagesStats";

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

  getOutbox(req: OutboxRequest): Observable<HttpResponse<MessageListResponse[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof InboxRequest]).join('&');
    console.log(queryString);
    const url = this.messageUrl + `/outbox?${queryString}`;

    return this.http.get<MessageListResponse[]>(url, {observe: "response"});
  }

  getMessage(req: number): Observable<MessageDetailResponse> {
    return this.http.get<MessageDetailResponse>(`${this.messageUrl}/${req}`);
  }

  getStats(): Observable<MessagesStats> {
    return this.http.get<MessagesStats>(`${this.messageUrl}/stats`);
  }

  checkForNew(): Observable<number> {
    return this.http.get<number>(`${this.messageUrl}/check-new`);
  }

  deleteMessage(req: number): Observable<HttpResponse<string>> {
    return this.http.delete<HttpResponse<string>>(`${this.messageUrl}/${req}`);
  }
}
