import {Component, EventEmitter, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-messages-nav',
  templateUrl: './messages-nav.component.html',
  styleUrls: ['./messages-nav.component.css']
})
export class MessagesNavComponent implements OnInit {
  @Output() selection = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
  }

  selectMenu(value: string) {
    console.log(value);
    this.selection.emit(value);
  }

}
