import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormGroup,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from "@angular/forms";
import { Location } from '@angular/common';
import {map} from "rxjs/operators";
import {of} from "rxjs";
import {UserService} from "../../../Services/user.service";
import {Router} from "@angular/router";
import {MessageService} from "../../../Services/message.service";
import {SendMessageRequest} from "../../../model/messages/SendMessageRequest";
import {ReplyOfState} from "../../../model/messages/ReplyOfState";
import Swal from "sweetalert2";
import {SwalService} from "../../../Services/swal.service";

@Component({
  selector: 'app-message-send',
  templateUrl: './message-send.component.html',
  styleUrls: ['./message-send.component.css']
})
export class MessageSendComponent implements OnInit {
  sendForm: FormGroup;
  inReplyOf: ReplyOfState | undefined;

  constructor(private userService : UserService, private router: Router,
              private messageService: MessageService, private location:Location, private swalService: SwalService) { }

  existingUsernameValidator: AsyncValidatorFn = (control: AbstractControl) => {
    return control?.value !== ""
      ? this.userService
        .checkUsernameExistence(control.value)
        .pipe(
          map((exists) =>
            exists
              ? null
              : {existingUsername: true}
          )
        )
      : of(null);
  }

  ngOnInit(): void {
    let state = this.location.getState() as any;

    this.inReplyOf = state['inReplyOf'];

    this.sendForm = new UntypedFormGroup({
      'receiverUsername': new UntypedFormControl(null, {
          validators: [
            Validators.required
          ],
          asyncValidators: this.existingUsernameValidator,
          updateOn: 'blur'
        }
      ),
      'subject': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ]
        }
      ),
      'body': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ]
        }
      )
    });

    if(this.inReplyOf !== undefined){
      this.receiverUsername?.setValue(this.inReplyOf.usernameTo);
      this.receiverUsername?.disable();
      this.subject?.setValue(`Re: ${this.inReplyOf.messageSubject}`);
    }
  }

  get receiverUsername() { return this.sendForm?.get('receiverUsername'); }
  get subject() { return this.sendForm?.get('subject'); }
  get body() { return this.sendForm?.get('body'); }

  onSubmit() {
    if(this.sendForm?.invalid){
      alert("Invalid form!");
      return;
    }

    let reg: SendMessageRequest = new SendMessageRequest();
    reg.receiverUsername = this.receiverUsername?.value;
    reg.body = this.body?.value;
    reg.subject = this.subject?.value;

    if(this.inReplyOf !== undefined){
      reg.replyForId = this.inReplyOf.messageId;
    }

    console.log(reg);

    let that = this;
    this.messageService.sendMessage(reg).subscribe({
      next(response) {
        Swal.fire({
          ...that.swalService.BootstrapConfirmOnlyOptions,
          icon: 'success',
          html: response
        }).then(() => that.router.navigate(['/messages']))
      },
      error(error) {
        Swal.fire({
          ...that.swalService.BootstrapConfirmOnlyOptions,
          icon: 'error',
          html: error
        })
      },
      complete() {
        // .then(resp => console.log(resp));
      }
    })

  }

}
