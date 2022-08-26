import {Component, Input, OnInit} from '@angular/core';
import {AdminService} from "../../../Services/admin.service";

@Component({
  selector: 'app-export-data',
  templateUrl: './export-data.component.html',
  styleUrls: ['./export-data.component.css']
})
export class ExportDataComponent implements OnInit {
  @Input() itemIds: number[] = [];
  data: string;
  loading: boolean = false;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
  }

  export(type: string) {
    this.loading = true;

    this.adminService.exportData(type, this.itemIds).subscribe({
      next: value => {
        this.data = value;
      },
      complete: () => {
        this.loading = false;
      }
    })
  }

}
