import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-user-details-line',
  templateUrl: './user-details-line.component.html',
  styleUrls: ['./user-details-line.component.css']
})

export class UserDetailsLineComponent implements OnInit {
  @Input() title: string | undefined;
  @Input() value: string | undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
