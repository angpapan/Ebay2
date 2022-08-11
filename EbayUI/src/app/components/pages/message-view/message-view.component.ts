import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {MessageService} from "../../../Services/message.service";
import {MessageDetailResponse} from "../../../model/messages/MessageDetailResponse";
import {ReplyOfState} from "../../../model/messages/ReplyOfState";
import {storageItems} from "../../../model/storageItems";
import Swal from "sweetalert2";
import {SwalService} from "../../../Services/swal.service";

@Component({
  selector: 'app-message-view',
  templateUrl: './message-view.component.html',
  styleUrls: ['./message-view.component.css']
})
export class MessageViewComponent implements OnInit {
  message: MessageDetailResponse;
  messageId: number;
  inReplyOf: ReplyOfState;
  userUsername: string;

  constructor(private messageService: MessageService, private route: ActivatedRoute,
              private swalService: SwalService, private router: Router){}

  ngOnInit(): void {
    this.userUsername = localStorage.getItem(storageItems.Username)!;
    this.inReplyOf = new ReplyOfState();

    this.route.params.subscribe(params => {
      this.messageId = params['id'];
      this.messageService.getMessage(this.messageId).subscribe(msg => {
        this.message = msg
        console.log(this.message);

        this.inReplyOf.messageId = this.messageId;
        this.inReplyOf.messageSubject = msg.subject;
        this.inReplyOf.usernameTo = msg.usernameFrom;
      });
    })
  }

  deleteMessage() {
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon: 'question',
      html: 'Do you want to delete this message?'
    }).then(resp => {
      if(resp.isConfirmed){
        this.messageService.deleteMessage(this.messageId).subscribe({
          next: (resp) => {
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              icon: 'success',
              html: String(resp)
            }).then(() => {
                this.router.navigate(['/messages']);
            })
          }
        })
      }
    });
  }

}
