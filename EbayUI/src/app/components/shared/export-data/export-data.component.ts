import {Component, Input, OnInit} from '@angular/core';
import {AdminService} from "../../../Services/admin.service";
import { Popover } from 'bootstrap';
import {storageItems} from "../../../model/storageItems";

@Component({
  selector: 'app-export-data',
  templateUrl: './export-data.component.html',
  styleUrls: ['./export-data.component.css']
})
export class ExportDataComponent implements OnInit {
  @Input() itemIds: number[] = [];
  @Input() placement: "auto" | "top" | "bottom" | "left" | "right" | (() => void) | undefined = "auto";
  data: string;
  loading: boolean = false;
  dataType: string = "xml";
  showCopy: boolean = false;
  role: string | null;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.role = localStorage.getItem(storageItems.Role);
    console.log(this.role)

    const options: Partial<Popover.Options> = {
      html: true,
      title: "Export Items Data",
      content: document.getElementById("export-popover-content") as Element,
      placement: this.placement,
      container: 'body',
    }
    const exampleEl: HTMLElement = document.getElementById('export-button-popover')!;
    new Popover(exampleEl, options)
  }

  export() {
    this.loading = true;

    this.adminService.exportData(this.dataType, this.itemIds).subscribe({
      next: value => {
        this.data = value;
      },
      complete: () => {
        this.loading = false;
      }
    })
  }

  selectType(dataType: string){
    if(this.dataType !== dataType){
      this.dataType = dataType;
      this.export();
    }
  }

  copyData(): void{
    navigator.clipboard.writeText(this.data).then(() => {
      console.log("Copied")
      this.showCopy = true;
      setTimeout(() => {
        this.showCopy = false;
      }, 2000);
    })
  }


}
