import { Component, OnInit } from '@angular/core';
import {SwalService} from "../../../Services/swal.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-export-data-page',
  templateUrl: './export-data-page.component.html',
  styleUrls: ['./export-data-page.component.css']
})
export class ExportDataPageComponent implements OnInit {
  items: number[] = [];
  from: number;
  to: number;
  specificString: string = "";
  method: string = "range";

  constructor(private swalService: SwalService) { }

  ngOnInit(): void {
  }

  setRange() {
    this.items = [];
    let to = this.to ?? 1;
    let from = this.from ?? 1;

    if(to - from + 1 > 600){
      this.throwBigNumberError();
      return;
    }

    for(let i = from; i <= to; i++){
      this.items.push(i);
    }
  }

  setSpecific() {
    this.items = [];
    let temp: string[] = this.specificString.split(',');
    if(temp.length > 600){
      this.throwBigNumberError();
      return;
    }


    for(let id of temp){
      const i = Number(id);
      if(isNaN(i)){
        this.items = [];
        Swal.fire({
          ...this.swalService.BootstrapConfirmOnlyOptions,
          icon: 'error',
          html: 'Please only enter comma separated numbers.'
        });
        return;
      }

      this.items.push(i);
    }

  }

  throwBigNumberError() {
    Swal.fire({
      ...this.swalService.BootstrapConfirmOnlyOptions,
      icon: 'error',
      html: 'The maximum items number is 600. Please select fewer items to display.'
    });
  }

  selectMethod(method: string){
    this.items = [];
    this.method = method;
  }

}
