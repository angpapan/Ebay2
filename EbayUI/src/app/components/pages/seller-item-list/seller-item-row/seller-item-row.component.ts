import {AfterContentInit, AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {SellerItem} from "../../../../model/Items/SellerItem";
import Swal from "sweetalert2";
import {ItemService} from "../../../../Services/item.service";
import {SwalService} from "../../../../Services/swal.service";

@Component({
  selector: '[app-seller-item-row]',
  templateUrl: './seller-item-row.component.html',
  styleUrls: ['./seller-item-row.component.css']
})
export class SellerItemRowComponent implements AfterViewInit {
  @Input() item: SellerItem;
  visible: boolean = true;

  constructor(private itemService: ItemService, private swalService: SwalService) { }

  ngAfterViewInit(): void {
    let source: any;

    if(this.item.image !== undefined && this.item.image !== null){
      const byteArray = new Uint8Array(atob(this.item.image!).split('').map(char => char.charCodeAt(0)));
      console.log(byteArray);
      let bl: Blob = new Blob([byteArray]);
      source = window.URL.createObjectURL(bl);
    }
    else {
      source = "../../../../../assets/no-image-available.jpg";
    }

    let postPreviewImage = <HTMLInputElement>document.getElementById(`item-thumbnail-${this.item.itemId}`);
    postPreviewImage.src = source;
  }


  startAuction(id: number) {
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon: 'question',
      html: 'Are you sure that you want to start this auction?'
    }).then(result => {
      if(result.isConfirmed){
        this.itemService.startAuction(this.item.itemId).subscribe({
          next: value => {
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              icon: 'success',
              html: 'Auction started!'
            }) ;
            this.item.started = new Date();
          }
        })
      }
    })
  }

  deleteAuction(id: number) {
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon: 'warning',
      html: 'Are you sure that you want to delete this auction?'
    }).then(result => {
      if(result.isConfirmed){
        this.itemService.deleteAuction(this.item.itemId).subscribe({
          next: value => {
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              icon: 'success',
              html: 'Auction deleted!'
            }) ;
            this.visible = false;
          }
        })
      }
    })
  }
}
