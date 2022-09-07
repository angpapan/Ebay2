import {Component, OnInit, ViewChild} from '@angular/core';
import {Gallery, GalleryItem, ImageItem} from "ng-gallery";
import {ItemService} from "../../../Services/item.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ItemDetailsFull} from "../../../model/Items/ItemDetailedFull";
import {ItemViewComponent} from "../item-view/item-view.component";

@Component({
  selector: 'app-item-full-view',
  templateUrl: './item-full-view.component.html',
  styleUrls: ['./item-full-view.component.css']
})

export class ItemFullViewComponent implements OnInit {

  @ViewChild(ItemViewComponent) itemView : any ;
  item: ItemDetailsFull;
  images: GalleryItem[] = [];
  noImg = '../../../assets/no-image-available.jpg';
  bid: number;
  informations:any;

  constructor(private itemService: ItemService, private route: ActivatedRoute, private router: Router, private gallery: Gallery) {
  }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      this.itemService.getItemFullDetails(params['id']).subscribe({
        next: item => {
          this.item = item;

          if (this.item.images !== undefined && this.item.images !== null) {
            this.item.images.forEach(img => {
              const byteArray = new Uint8Array(atob(img.data!).split('').map(char => char.charCodeAt(0)));

              let bl: Blob = new Blob([byteArray], {type: 'application/pdf'});
              let source = window.URL.createObjectURL(bl);

              this.images.push(new ImageItem({src: source, thumb: source}));
            });
          } else
            this.images.push(new ImageItem({src: this.noImg, thumb: this.noImg}));


          this.item.bids = this.item.bids?.sort((a,b) => { return (a.amount > b.amount) ? 1 : -1; });
          if( (this.item.buyPrice !== null && this.item.buyPrice !== undefined && this.item.buyPrice == this.item.price) ||
            new Date(this.item.ends) < new Date() ) {
            this.item.completed = 1;
            let maxBid = this.item.bids==undefined?null:this.item.bids[0];
            if( maxBid ) {
              this.item.buyer = maxBid.bidder.userId;
              this.item.ended = maxBid.time;
            }
          }
          else{
            this.item.completed = (this.item.started === null || this.item.started === undefined) ? 3 : 2;
          }



          },
        error: err => {
          this.router.navigate([`/home`]).then()
        }
      });
    });

  }

  createInfo():void{

  }

  previewItem(): void{
    this.itemView.makePreview();
  }
}
