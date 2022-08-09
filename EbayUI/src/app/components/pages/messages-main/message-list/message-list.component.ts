import {Component, Input, OnInit, SimpleChanges, OnDestroy, ViewChild, OnChanges, AfterViewInit} from '@angular/core';
import {PaginationResponseHeader} from "../../../../model/PaginationResponseHeader";
import {MessageListResponse} from "../../../../model/messages/MessageListResponse";
import {MessageService} from "../../../../Services/message.service";
import {InboxRequest} from "../../../../model/messages/InboxRequest";
import {OutboxRequest} from "../../../../model/messages/OutboxRequest";
import {Observable, Subject} from "rxjs";
import {HttpResponse} from "@angular/common/http";
import {DataTableDirective} from "angular-datatables";
import {ADTSettings} from "angular-datatables/src/models/settings";

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.css']
})
export class MessageListComponent implements OnInit, OnDestroy, OnChanges, AfterViewInit {
  @Input() selectedMenu: string;

  @ViewChild(DataTableDirective, {static: false})
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<ADTSettings> = new Subject<ADTSettings>();

  messages: MessageListResponse[] = [];

  constructor(private messageService: MessageService) { }

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes);
    if(changes['selectedMenu'].previousValue !== undefined && changes['selectedMenu'].previousValue !== changes['selectedMenu'].currentValue){
      this.selectedMenu = changes['selectedMenu'].currentValue;
      this.messages = [];

      // rerender datatable
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        dtInstance.destroy();
        this.dtTrigger.next(this.dtOptions);
      })
    }
  }

  ngOnInit(): void {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      serverSide: true,
      processing: true,
      order:[[2, "desc"]],
      ajax: (dataTablesParameters: any, callback) => {
        let req: InboxRequest | OutboxRequest;
        if(that.selectedMenu === 'inbox'){
          req = new InboxRequest();
        }
        else if(that.selectedMenu === 'outbox'){
          req = new OutboxRequest();
        }
        else {
          return;
        }

        req.pageNumber = (dataTablesParameters.start / dataTablesParameters.length) + 1;
        req.pageSize = dataTablesParameters.length;
        const searchValue = dataTablesParameters.search.value;
        if(searchValue !== undefined && searchValue !== null && searchValue !== "")
          req.search = searchValue;

        let obs: Observable<HttpResponse<MessageListResponse[]>> =
          (that.selectedMenu === 'inbox') ?
            this.messageService.getInbox(req) :
            this.messageService.getOutbox(req);

        obs.subscribe({
          next: resp => {
            console.log(resp.headers.get('X-pagination'));
            console.log(resp);
            let pagination: PaginationResponseHeader = JSON.parse(resp.headers.get('X-pagination')!);
            console.log(pagination);

            this.messages = resp.body!;

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
      columns: [{ data: 'usernameFrom' }, { data: 'subject' }, { data: 'timeSent' }]
    };
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next(this.dtOptions);
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }
}
