import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';


import { UserService } from './Services/user.service';
import { AuthGuard } from './guards/auth.guard';
import { ErrorInterceptor} from './helpers/error.interceptor';
import { JwtInterceptor} from './helpers/jwt.interceptor';
import {CommonModule} from "@angular/common";
import {RegisterComponent} from "./components/shared/register/register.component";
import {PasswordsMatchValidatorDirective} from "./components/shared/register/passwords-match-validator.directive";
import {LoginComponent} from "./components/shared/login/login.component";
import {WelcomeComponent} from "./components/pages/welcome/welcome.component";
import {NavMenuComponent} from "./components/shared/nav-menu/nav-menu.component";
import {UserComponent} from "./components/pages/user/user.component";
import {CategoryNavComponent} from "./components/shared/category-nav/category-nav.component";
import { ContainerComponent } from './components/shared/container/container.component';
import { UserNavBarComponent } from './components/pages/user/user-nav-bar/user-nav-bar.component';
import { UserDetailsComponent } from './components/pages/user/user-details/user-details.component';
import { UserDetailsLineComponent } from './components/pages/user/user-details/user-details-line/user-details-line.component';
import { UserListComponent } from './components/pages/user-list/user-list.component';
import {StarRatingModule} from "angular-star-rating";
import {AdminGuard} from "./guards/admin.guard";
import {DataTablesModule} from "angular-datatables";
import { PendingVerificationComponent } from './components/pages/pending-verification/pending-verification.component';
import {HomeComponent} from "./components/pages/home/home.component";
import { ItemBoxComponent } from './components/pages/home/item-box/item-box.component';
import { RegisterPageComponent } from './components/pages/register-page/register-page.component';
import { LoginPageComponent } from './components/pages/login-page/login-page.component';
import {NotVerifiedGuard} from "./guards/not-verified.guard";
import {LoginGuard} from "./guards/login.guard";
import { MessagesMainComponent } from './components/pages/messages-main/messages-main.component';
import { MessagesNavComponent } from './components/pages/messages-main/messages-nav/messages-nav.component';
import { MessageListComponent } from './components/pages/messages-main/message-list/message-list.component';
import { MessageViewComponent } from './components/pages/message-view/message-view.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FetchDataComponent,
    UserComponent,
    LoginComponent,
    CategoryNavComponent,
    WelcomeComponent,
    RegisterComponent,
    PasswordsMatchValidatorDirective,
    ContainerComponent,
    UserNavBarComponent,
    UserDetailsComponent,
    UserDetailsLineComponent,
    UserListComponent,
    PendingVerificationComponent,
    HomeComponent,
    ItemBoxComponent,
    RegisterPageComponent,
    LoginPageComponent,
    MessagesMainComponent,
    MessagesNavComponent,
    MessageListComponent,
    MessageViewComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    CommonModule,
    RouterModule.forRoot([
      // { path: '', component: HomeComponent, pathMatch: 'full' },
      {path: 'fetch-data', component: FetchDataComponent},
      {path: '', component: WelcomeComponent, pathMatch: 'full'},
      {path: 'users', component: UserListComponent, canActivate: [AuthGuard, AdminGuard]},
      {path: 'users/:username', component: UserComponent, canActivate: [AuthGuard]},
      {path: 'login', component: LoginPageComponent, canActivate:[LoginGuard]},
      {path: 'register', component: RegisterPageComponent},
      {path: 'not-verified', component: PendingVerificationComponent, canActivate:[NotVerifiedGuard]},
      {path: 'home', component: HomeComponent},
      {path: 'messages', component: MessagesMainComponent},
      {path: 'messages/:id', component: MessageViewComponent},
      {path: '**', redirectTo: ''}
    ]),
    ReactiveFormsModule,
    StarRatingModule.forRoot(),
    DataTablesModule
  ],
  providers: [
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
