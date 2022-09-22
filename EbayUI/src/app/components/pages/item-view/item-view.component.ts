import {Component, Input, OnInit, ViewChild} from '@angular/core';

import   Swal  from 'sweetalert2';
import { Gallery , GalleryItem , ImageItem } from 'ng-gallery';
import { ItemService } from "../../../Services/item.service";
import { ItemDetails } from "../../../model/Items/ItemDetailed";
import {ActivatedRoute, Router} from "@angular/router";
import {Subject, take, takeUntil} from "rxjs";
import { MapComponent} from "../../shared/map/map.component";

@Component({
  selector: 'app-item-view',
  templateUrl: './item-view.component.html',
  styleUrls: ['./item-view.component.css']
})
export class ItemViewComponent implements OnInit {

  @ViewChild(MapComponent) childMap:any;
  @Input() preview  = false;
  item : ItemDetails;
  images: GalleryItem[] = [];
  noImg = '../../../assets/no-image-available.jpg';
  bid : number;
  constructor(private itemService : ItemService, private route : ActivatedRoute, private router : Router, private gallery : Gallery) { }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
        this.itemService.getItemDetails(params['id']).subscribe({ next: item => {
          this.item = item;

          if (this.item.images != undefined && this.item.images?.length>0) {
            console.log("item has image")
            console.log(this.item.images);
            console.log("end");
            this.item.images.forEach(img => {
              const byteArray = new Uint8Array(atob(img!).split('').map(char => char.charCodeAt(0)));
              //console.log(byteArray);
              let bl: Blob = new Blob([byteArray]);
              let source = window.URL.createObjectURL(bl);

              this.images.push(new ImageItem({src: source, thumb: source}));
            });
          }
          else
            this.images.push(new ImageItem({src: this.noImg, thumb: this.noImg}));
        },
          error: err => { this.router.navigate([`/home`]).then() }
    });
    });
  }


  placeBid() : void{
    if(this.preview)
      return;

    if(this.bid != undefined && this.bid > this.item.price){
      if(this.item.buyPrice != undefined && this.bid > this.item.buyPrice){
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          html: 'You can buy it now for ' + this.item.buyPrice,
        })
      }
      else{
        Swal.fire({
          title: 'Do you want to place the bid?',
          showCancelButton: true,
          confirmButtonText: 'Yes',
        }).then(result => {
          if (result.isConfirmed) {
            this.itemService.placebid(this.item.itemId,this.bid).subscribe({
              next: value =>{
                Swal.fire('Ok', 'You bid '+ this.bid + '&#8364;' + ' for ' + this.item.name ,  'success')
                  .then(result => { this.item.price = this.bid ;  (this.bid as number|undefined ) = undefined});
              }
            })
          }
        })
      }

    }
    else{
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        html: 'Your bid amount must be greater than ' + this.item.price,
      })

    }

  }

  buyItem(): void {
    if(this.preview)
      return;
    if(this.item.buyPrice !== undefined && this.item.buyPrice !== null){
      this.bid = this.item.buyPrice;
      this.placeBid();
    }

  }

  resize(){
    this.childMap.resize();
  }

  makePreview(){
    this.preview = true;
  }



}
