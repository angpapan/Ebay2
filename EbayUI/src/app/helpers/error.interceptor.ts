import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from '../Services/authentication.service';
import {SwalService} from "../Services/swal.service";


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService, private swalService: SwalService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(err => {
      console.log(err);
      if (err.status === 401 || err.status === 403) {
        // auto logout if 401 response returned from api
        this.authenticationService.logout();
        location.reload();
      }
      const error = err.error.Message || err.statusText;
      this.swalService.errorAlert('Error', error);
      // alert(error);
      return throwError(error);
    }));
  }
}
