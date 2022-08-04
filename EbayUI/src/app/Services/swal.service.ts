import { Injectable } from '@angular/core';
import Swal from "sweetalert2";

@Injectable({
  providedIn: 'root'
})
export class SwalService {

  constructor() { }

  public BootstrapOptions = {
    customClass: {
      confirmButton: 'btn btn-primary mx-5',
      cancelButton: 'btn btn-danger mx-5',
    },
    buttonsStyling: false,
    showCancelButton: true,
    cancelButtonText: 'Ακύρωση',
    heightAuto: false,
  }

  public BootstrapConfirmOnlyOptions = {
    ...this.BootstrapOptions,
    showCancelButton: false,
  }

  public errorAlert = (title: string, text: string) => {
    Swal.fire({
      ...this.BootstrapConfirmOnlyOptions,
      icon: 'error',
      title: title,
      html: text
    });
  }
}




