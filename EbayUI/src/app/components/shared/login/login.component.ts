import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import {AuthenticationService} from "../../../Services/authentication.service";
import {storageItems} from "../../../model/storageItems";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loading = false;

  loginForm = this.formBuilder.group({
    username: '',
    password: ''
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private formBuilder: UntypedFormBuilder
  ) { }

  ngOnInit(): void {
    // this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login(): void {
    this.loading = true;
    this.authenticationService.login(this.loginForm.value.username, this.loginForm.value.password)
      .subscribe({
        next: response =>
          {
            console.log(response.body);
            localStorage.setItem(storageItems.Token, <string>response.body?.token);
            localStorage.setItem(storageItems.Username, <string>response.body?.username);
            localStorage.setItem(storageItems.Role, <string>response.body?.role);
            localStorage.setItem(storageItems.Enabled, String(<boolean>response.body?.enabled));

            if(response.body?.role === "admin")
              this.router.navigate(['/users']);
            else if(response.body?.enabled === false)
              this.router.navigate(['/not-verified']);
            else
              this.router.navigate(['/home']);
          }
        ,
        error: error => {
            this.loading = false
          },
        }
      );

    this.loginForm.reset();
  }
}
