import { Component, OnInit } from '@angular/core';
import {SellerItem} from "../../../model/Items/SellerItem";
import {UserListRequest} from "../../../model/UserListRequest";
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";
import {ItemService} from "../../../Services/item.service";
import {SellerListRequest} from "../../../model/Items/SellerListRequest";

@Component({
  selector: 'app-seller-item-list',
  templateUrl: './seller-item-list.component.html',
  styleUrls: ['./seller-item-list.component.css']
})
export class SellerItemListComponent implements OnInit {
  dtOptions: DataTables.Settings = {};
  items: SellerItem[] = [];

  constructor(private itemService: ItemService) { }

  ngOnInit(): void {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      // pageLength: 1,
      // searchDelay: 10000,
      serverSide: true,
      processing: true,
      order:[[0, "desc"]], //<--
      ajax: (dataTablesParameters: any, callback) => {
        console.log(dataTablesParameters);

        let req: SellerListRequest = new SellerListRequest();
        req.pageNumber = (dataTablesParameters.start / dataTablesParameters.length) + 1;
        req.pageSize = dataTablesParameters.length;
        const ordering = dataTablesParameters.order;
        const orderColumn = dataTablesParameters.columns[ordering[0].column].data + ' ' + ordering[0].dir;
        req.orderBy = orderColumn;
        const searchValue = dataTablesParameters.search.value;
        if(searchValue !== undefined && searchValue !== null && searchValue !== "")
          req.search = searchValue;

        that.itemService.getSellerItems(req).subscribe({
          next: resp => {
            console.log(resp.headers.get('X-pagination'));
            console.log(resp);
            let pagination: PaginationResponseHeader = JSON.parse(resp.headers.get('X-pagination')!);
            console.log(pagination);

            that.items = resp.body!;

            callback({
              recordsTotal: pagination.TotalCount,
              // recordsDisplay: 10,
              // draw: pagination.CurrentPage,
              recordsFiltered: pagination.TotalCount,
              data: []
            });
          }
        });
      },
      columns: [{ data: 'itemId', visible:false }, { data: 'image', orderable:false }, { data: 'info', orderable: false },
        { data: 'price' }, { data: 'ends' }, { data: 'actions', orderable: false }]


    };
  }

}
