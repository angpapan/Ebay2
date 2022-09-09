import { Component, OnInit } from '@angular/core';
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";
import {BidderListRequest} from "../../../model/Items/BidderListRequest";
import {BidService} from "../../../Services/bid.service";
import {BidderItem} from "../../../model/Items/BidderItem";

@Component({
  selector: 'app-bidder-item-list',
  templateUrl: './bidder-item-list.component.html',
  styleUrls: ['./bidder-item-list.component.css']
})
export class BidderItemListComponent implements OnInit {
  dtOptions: DataTables.Settings = {};
  items: BidderItem[] = [];

  constructor(private bidService: BidService) {}

  ngOnInit(): void {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      serverSide: true,
      processing: true,
      order: [[6, "asc"]], //<--
      ajax: (dataTablesParameters: any, callback) => {
        console.log(dataTablesParameters);

        let req: BidderListRequest = new BidderListRequest();
        req.pageNumber = (dataTablesParameters.start / dataTablesParameters.length) + 1;
        req.pageSize = dataTablesParameters.length;
        const ordering = dataTablesParameters.order;
        const orderColumn = dataTablesParameters.columns[ordering[0].column].data + ' ' + ordering[0].dir;
        req.orderBy = orderColumn;
        const searchValue = dataTablesParameters.search.value;
        if (searchValue !== undefined && searchValue !== null && searchValue !== "")
          req.search = searchValue;

        that.bidService.getBidderItems(req).subscribe({
          next: resp => {
            console.log(resp.headers.get('X-pagination'));
            console.log(resp);
            let pagination: PaginationResponseHeader = JSON.parse(resp.headers.get('X-pagination')!);
            console.log(pagination);

            that.items = resp.body!;

            callback({
              recordsTotal: pagination.TotalCount,
              recordsFiltered: pagination.TotalCount,
              data: []
            });
          }
        });
      },
      columns: [{data: 'itemId', visible: false}, {data: 'image', orderable: false}, {data: 'info', orderable: false},
        {data: 'maxBid'}, {data: 'userMaxBid'}, {data: 'buyPrice'}, {data: 'ends'}, {data: 'actions', orderable: false}]


    };
  }
}
