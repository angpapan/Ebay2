import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserListRequest} from "../model/UserListRequest";
import {UserListItem} from "../model/UserListItem";
import {SendMessageRequest} from "../model/messages/SendMessageRequest";
import {InboxRequest} from "../model/messages/InboxRequest";
import {MessageDetailResponse} from "../model/messages/MessageDetailResponse";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  private messageUrl = 'https://localhost:7088/message';

  sendMessage(req: SendMessageRequest): Observable<string> {
    return this.http.post<string>(this.messageUrl + '/send', req,);
  }

  getInbox(req: InboxRequest): Observable<HttpResponse<MessageDetailResponse[]>> {
    let queryString = Object.keys(req).map(key => key + '=' + req[key as keyof InboxRequest]).join('&');
    console.log(queryString);
    const url = this.messageUrl + `/inbox?${queryString}`;

    return this.http.get<MessageDetailResponse[]>(url, {observe: "response"});
  }

  // TODO complete message services
}
