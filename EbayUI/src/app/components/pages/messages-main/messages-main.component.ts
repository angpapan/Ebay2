import { Component, OnInit } from '@angular/core';
import {UserListItem} from "../../../model/UserListItem";
import {AdminService} from "../../../Services/admin.service";
import {UserListRequest} from "../../../model/UserListRequest";
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";

@Component({
  selector: 'app-messages-main',
  templateUrl: './messages-main.component.html',
  styleUrls: ['./messages-main.component.css']
})
export class MessagesMainComponent implements OnInit {
  selectedMenu: string = "inbox";

  ngOnInit() {
  }

  getSelection(value: string){
    this.selectedMenu = value;
  }
}
