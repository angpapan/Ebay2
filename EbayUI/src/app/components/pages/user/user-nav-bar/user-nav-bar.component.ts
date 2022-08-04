import {Component, Input, OnInit} from '@angular/core';
import {UserDetails} from "../../../../model/UserDetails";

@Component({
  selector: 'app-user-nav-bar',
  templateUrl: './user-nav-bar.component.html',
  styleUrls: ['./user-nav-bar.component.css']
})
export class UserNavBarComponent implements OnInit {

  @Input() user: UserDetails;
  @Input() showRating: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
