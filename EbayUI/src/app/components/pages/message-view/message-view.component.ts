import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MessageService} from "../../../Services/message.service";
import {MessageDetailResponse} from "../../../model/messages/MessageDetailResponse";

@Component({
  selector: 'app-message-view',
  templateUrl: './message-view.component.html',
  styleUrls: ['./message-view.component.css']
})
export class MessageViewComponent implements OnInit {
  message: MessageDetailResponse;

  constructor(private messageService: MessageService, private route: ActivatedRoute){}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.messageService.getMessage(params['id']).subscribe(msg => {
        this.message = msg
        console.log(this.message);
      });
    })
  }

}
