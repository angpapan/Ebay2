import {Component, OnInit, ViewChild} from '@angular/core';
import {ItemSimple} from "../../../model/Items/ItemSimple";
import {ItemService} from "../../../Services/item.service";
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";
import {ItemListRequest} from "../../../model/Items/ItemListRequest";
import {FilterBlockComponent} from "../../shared/filter-block/filter-block.component";
import {ActivatedRoute} from "@angular/router";
@Component({
  selector: 'app-result-search',
  templateUrl: './result-search.component.html',
  styleUrls: ['./result-search.component.css']
})
export class ResultSearchComponent implements OnInit {

  itemList: any ;
  hasMore: boolean;
  info : ItemListRequest;

  constructor(private _itemService : ItemService, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params=>{
      let req = new ItemListRequest();
      req.pageSize = 3;
      req.pageNumber = 1;
      req.serialize(params);
      this.info = req;
      this._itemService.getSearcItemList(req).subscribe({next:response=>{
          let pagination: PaginationResponseHeader = JSON.parse(response.headers.get('X-pagination')!);
          console.log(pagination);
          this.hasMore = pagination.HasNext;
          this.itemList = response.body!;
        }
      })

    })


  }

  loadMore(){
    if(this.hasMore){
      this.info.pageNumber ++;
      this._itemService.getSearcItemList(this.info).subscribe({next:response=>{
        let pageInfo : PaginationResponseHeader = JSON.parse(response.headers.get('X-pagination')!);
        console.log(pageInfo)
        this.hasMore = pageInfo.HasNext;
        this.itemList = this.itemList.concat(response.body);

      }})
    }
  }

}
