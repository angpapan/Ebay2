import { Component, OnInit } from '@angular/core';
import {UserListItem} from "../../../model/UserListItem";
import {AdminService} from "../../../Services/admin.service";
import {UserListRequest} from "../../../model/UserListRequest";
import {PaginationResponseHeader} from "../../../model/PaginationResponseHeader";
import {data} from "jquery";


@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  dtOptions: DataTables.Settings = {};
  users: UserListItem[] = [];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      // pageLength: 1,
      // searchDelay: 10000,
      serverSide: true,
      processing: true,
      order:[[4, "desc"]],
      ajax: (dataTablesParameters: any, callback) => {
        console.log(dataTablesParameters);

        let req: UserListRequest = new UserListRequest();
        req.pageNumber = dataTablesParameters.start+1;
        req.pageSize = dataTablesParameters.length;
        const ordering = dataTablesParameters.order;
        const orderColumn = dataTablesParameters.columns[ordering[0].column].data + ' ' + ordering[0].dir;
        req.orderBy = orderColumn;
        const searchValue = dataTablesParameters.search.value;
        if(searchValue !== undefined && searchValue !== null && searchValue !== "")
          req.search = searchValue;

        that.adminService.getAllUsers(req).subscribe({
          next: resp => {
            console.log(resp.headers.get('X-pagination'));
            console.log(resp);
            let pagination: PaginationResponseHeader = JSON.parse(resp.headers.get('X-pagination')!);
            console.log(pagination);

            that.users = resp.body!;

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
      columns: [{ data: 'username' }, { data: 'firstName' }, { data: 'lastName' },
        { data: 'email' }, { data: 'dateCreated' }, { data: 'enabled' }]


    };
  }
}
