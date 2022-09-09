import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { GalleryModule } from 'ng-gallery';

import { AppComponent } from './app.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';


import { UserService } from './Services/user.service';
import { AuthGuard } from './guards/auth.guard';
import { ErrorInterceptor} from './helpers/error.interceptor';
import { JwtInterceptor} from './helpers/jwt.interceptor';
import {CommonModule, CurrencyPipe} from "@angular/common";
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
import { MessageSendComponent } from './components/pages/message-send/message-send.component';
import {EnabledGuard} from "./guards/enabled.guard";
import { NewItemComponent } from './components/pages/new-item/new-item.component';
import { EditItemComponent } from './components/pages/edit-item/edit-item.component';
import { SellerItemListComponent } from './components/pages/seller-item-list/seller-item-list.component';
import { SellerItemRowComponent } from './components/pages/seller-item-list/seller-item-row/seller-item-row.component';

import { ItemViewComponent } from "./components/pages/item-view/item-view.component";
import { ItemSmallBoxComponent } from "./components/shared/item-small-box/item-small-box.component";
import {SellerAllItemsComponent} from "./components/pages/seller-all-items/seller-all-items.component";
import {
  SellerAllItemsRowComponent
} from "./components/pages/seller-all-items/seller-all-items-row/seller-all-items-row.component";
import {ItemFullViewComponent} from "./components/pages/item-full-view/item-full-view.component";
import {MapComponent} from "./components/shared/map/map.component";
import {ItemGridBlockComponent} from "./components/shared/item-grid-block/item-grid-block.component";
import {ResultSearchComponent} from "./components/pages/result-search/result-search.component";
import {FilterBlockComponent} from "./components/shared/filter-block/filter-block.component";

import { ExportDataComponent } from './components/shared/export-data/export-data.component';
import {MessageService} from "./Services/message.service";
import { BidderItemListComponent } from './components/pages/bidder-item-list/bidder-item-list.component';
import { BidderItemRowComponent } from './components/pages/bidder-item-list/bidder-item-row/bidder-item-row.component';

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
    MessageSendComponent,
    NewItemComponent,
    EditItemComponent,
    SellerItemListComponent,
    SellerItemRowComponent,
    ExportDataComponent,
    BidderItemListComponent,
    BidderItemRowComponent,
    ItemViewComponent,
    ItemSmallBoxComponent,
    SellerAllItemsComponent,
    SellerAllItemsRowComponent,
    ItemFullViewComponent,
    MapComponent,
    ItemGridBlockComponent,
    ResultSearchComponent,
    FilterBlockComponent,
    BidderItemListComponent,
    BidderItemRowComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule.forRoot([
      // { path: '', component: HomeComponent, pathMatch: 'full' },
      {path: 'fetch-data', component: FetchDataComponent},
      {path: '', component: WelcomeComponent, pathMatch: 'full'},
      {path: 'users', component: UserListComponent, canActivate: [AuthGuard, AdminGuard]},
      {path: 'users/:username', component: UserComponent, canActivate: [AuthGuard]},
      {path: 'login', component: LoginPageComponent, canActivate:[LoginGuard]},
      {path: 'register', component: RegisterPageComponent, canActivate:[LoginGuard]},
      {path: 'not-verified', component: PendingVerificationComponent, canActivate:[NotVerifiedGuard]},
      {path: 'home', component: HomeComponent},
      {path: 'messages', component: MessagesMainComponent, canActivate:[AuthGuard, EnabledGuard]},
      {path: 'messages/send', component: MessageSendComponent, canActivate:[AuthGuard, EnabledGuard]},
      {path: 'messages/:id', component: MessageViewComponent, canActivate: [AuthGuard, EnabledGuard] },
      { path: 'items/new', component: NewItemComponent, canActivate: [AuthGuard, EnabledGuard] },
      { path: 'items/seller-list', component: SellerItemListComponent, canActivate: [AuthGuard, EnabledGuard] },
      { path: 'items/bidder-list', component: BidderItemListComponent, canActivate: [AuthGuard, EnabledGuard] },
      { path: 'items/edit/:id', component: EditItemComponent, canActivate: [AuthGuard, EnabledGuard] },
      { path: 'items/:id', component: ItemViewComponent},
      { path: 'items/user/:username', component: SellerAllItemsComponent},
      { path: 'item/full/:id', component: ItemFullViewComponent },
      { path: 'search', component: ResultSearchComponent},
      {path: '**', redirectTo: ''}
    ]),
    ReactiveFormsModule,
    StarRatingModule.forRoot(),
    DataTablesModule,
    GalleryModule
  ],
  providers: [
    UserService,
    MessageService,
    CurrencyPipe,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
