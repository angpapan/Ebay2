import { Component, OnInit } from '@angular/core';
import {UserDetails} from "../../../model/UserDetails";
import {UserService} from "../../../Services/user.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  user: UserDetails = new UserDetails();

  constructor(private userService: UserService, private route: ActivatedRoute){}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userService.getUser(params['username']).subscribe(user => {
        this.user = user
        console.log(this.user);
      });
    })
  }

}
