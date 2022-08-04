import {Component, Input, OnInit} from '@angular/core';
import {UserDetails} from "../../../../model/UserDetails";
import {storageItems} from "../../../../model/storageItems";
import {AdminService} from "../../../../Services/admin.service";
import {SwalService} from "../../../../Services/swal.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  @Input() user: UserDetails;
  role : string | null = localStorage.getItem(storageItems.Role);

  constructor(private adminService: AdminService, private swalService: SwalService) { }

  ngOnInit(): void {
  }

  makeString(num: number | undefined): string | undefined{
    if(num == undefined)
      return num;

    return num!.toString();
  }

  verifyUser() {
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      title: 'Verify User',
      html: `Are you sure you want to verify user ${this.user.username}?`
    }).then(result => {
      if(result.isConfirmed){
        this.adminService.verifyUser(this.user.username).subscribe({
          next: response => {
            console.log(response);
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              title: 'Successful Verification',
              html: `${response.body}`
            })
            this.user.enabled = true;
          }
        })
      }
    })
  }

}
