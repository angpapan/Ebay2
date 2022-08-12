import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {MessagesStats} from "../../../../model/messages/MessagesStats";
import {MessageService} from "../../../../Services/message.service";

@Component({
  selector: 'app-messages-nav',
  templateUrl: './messages-nav.component.html',
  styleUrls: ['./messages-nav.component.css']
})
export class MessagesNavComponent implements OnInit {
  @Output() selection = new EventEmitter<string>();
  stats: MessagesStats | undefined;

  constructor(private messageService: MessageService) { }



  ngOnInit(): void {
    this.getStats();
  }

  getStats() {
    this.messageService.getStats().subscribe({
      next: value => {
        this.stats = value;
      }
    })
  }

  selectMenu(value: string) {
    this.getStats();
    this.selection.emit(value);
  }

}
