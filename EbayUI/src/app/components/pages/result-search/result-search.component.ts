import {Component, OnInit, ViewChild} from '@angular/core';
import {ItemService} from "../../../Services/item.service";
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";
import {ItemListRequest} from "../../../model/Items/ItemListRequest";
import {ActivatedRoute, Router} from "@angular/router";
import {FilterBlockComponent} from "../../shared/filter-block/filter-block.component";

@Component({
  selector: 'app-result-search',
  templateUrl: './result-search.component.html',
  styleUrls: ['./result-search.component.css']
})
export class ResultSearchComponent implements OnInit {

  @ViewChild(FilterBlockComponent) filters : any ;
  totalResults : number;
  itemList: any ;
  hasMore: boolean;
  info : ItemListRequest;
  sortList : orderList = new orderList(["Name" , "Price" , "Ends"]);

  constructor(private _itemService : ItemService, private route : ActivatedRoute, private router : Router) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params=>{
      console.log("params", params);

      if(params["orderBy"] === undefined){
        this.sortList.active = "";
      }

      let req = new ItemListRequest();
      req.pageSize = 30;
      req.pageNumber = 1;
      req.serialize(params);
      this.info = req;
      console.log(req);

      this.sortList.setNewSort(req.orderBy);

      this._itemService.getSearcItemList(req).subscribe({next:response=>{
          let pagination: PaginationResponseHeader = JSON.parse(response.headers.get('X-pagination')!);
          //console.log(pagination);
          this.totalResults = pagination.TotalCount;
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
          //console.log(pageInfo)
          this.hasMore = pageInfo.HasNext;
          this.itemList = this.itemList.concat(response.body);

        }})
    }
  }

  sort(newSort : string){
    this.info.pageNumber = 1;
    this.info.orderBy = newSort;
    this.router.navigate( [`search`], {queryParams: this.info.reduceParameters()} ).then();
  }


}

class orderList {
  list: any [] = [];
  inActive: any [];
  active : string;

  constructor(_list: string[]) {
    _list.forEach(value => this.list.push({'name': value, 'order': ' asc', 'uni': '\u2191', 'actv': false},
      {'name': value, 'order': ' desc', 'uni': '\u2193', 'actv': false}));
    this.inActive = this.list;
  }


  setNewSort(newSort: string | undefined) {
    if (newSort == undefined)
      return;

    /* remove active */
    let index = this.list.findIndex((i=>i.actv == true));
    if(index>=0) {
      this.list[index].actv = false;
    }

    /* set new active */
    let orderL = newSort.split(" ");
    this.list.find(value => value.name === orderL[0] && value.order.endsWith(orderL[2]))['actv'] = true;

    /* set new options */
    this.inActive = this.list.filter(i => i.actv == false);
    /* set new preview */
    let t =  this.list.find(i => i.actv == true);
    this.active = t.name + " " + t.uni;

  }


}

