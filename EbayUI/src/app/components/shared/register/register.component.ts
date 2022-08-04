import { Component, OnInit } from '@angular/core';
import {
  AbstractControl, AsyncValidatorFn,
  UntypedFormControl,
  UntypedFormGroup,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {of} from "rxjs";
import {map} from "rxjs/operators";
import {UserService} from "../../../Services/user.service";
import {UserRegisterRequest} from "../../../model/UserRegisterRequest";
import {Router} from "@angular/router";
// import {passwordsMatchValidator} from "./passwords-match-validator.directive";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm : UntypedFormGroup;
  showPassword: boolean = false;
  showVerifyPassword: boolean = false;
  activeTab: number = 1;



  constructor(private userService : UserService, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = new UntypedFormGroup({
      'username': new UntypedFormControl(null, {
          validators: [
            Validators.required,
            Validators.minLength(4)
          ],
          asyncValidators: this.uniqueUsernameValidator,
          updateOn: 'blur'
        }
      ),
      'password': new UntypedFormControl("", {
          validators: [
            Validators.required,
            Validators.minLength(8),
          ],
          updateOn: 'blur'
        }
      ),
      'verifyPassword': new UntypedFormControl("", {
          validators: [
            Validators.required,
            this.passwordsMatchValidator
          ],
          updateOn: 'blur'
        }
      ),
      'firstName': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'lastName': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'VATNumber': new UntypedFormControl("", {
          validators: [
            Validators.required,
            Validators.maxLength(14)
          ],
        }
      ),
      'email': new UntypedFormControl("", {
          validators: [
            Validators.required,
            Validators.email,
          ],
        }
      ),
      'phoneNumber': new UntypedFormControl("", {
          validators: [
            Validators.required,
            Validators.minLength(10),
            Validators.maxLength(10),
          ],
          updateOn: 'blur'
        }
      ),
      'country': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'city': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'street': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'streetNumber': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'postalCode': new UntypedFormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
    });
  }

  onSubmit() {
    if(this.registerForm?.invalid){
      alert("Invalid form!");
      return;
    }

    let reg: UserRegisterRequest = this.registerForm!.value;

    console.log(reg);

    let success: boolean = false;

    let that = this;
    this.userService.registerUser(reg).subscribe({
      next(response) {
        console.log(response);
        success = true;
      },
      error(error) {
        console.log(error)
      },
      complete() {
        that.router.navigate(['/not-verified']).then(resp => console.log(resp));
      }
    })

  }

  get username() { return this.registerForm?.get('username'); }
  get password() { return this.registerForm?.get('password'); }
  get verifyPassword() { return this.registerForm?.get('verifyPassword'); }
  get firstName() { return this.registerForm?.get('firstName'); }
  get lastName() { return this.registerForm?.get('lastName'); }
  get VATNumber() { return this.registerForm?.get('VATNumber'); }
  get email() { return this.registerForm?.get('email'); }
  get phoneNumber() { return this.registerForm?.get('phoneNumber'); }
  get country() { return this.registerForm?.get('country'); }
  get city() { return this.registerForm?.get('city'); }
  get street() { return this.registerForm?.get('street'); }
  get streetNumber() { return this.registerForm?.get('streetNumber'); }
  get postalCode() { return this.registerForm?.get('postalCode'); }

  passwordsMatchValidator: ValidatorFn = (control: AbstractControl) => {
    const { value } = control;
    const match = value === this?.password?.value;
    return match ? null : { passwordsMatch: true };
  };

  uniqueUsernameValidator: AsyncValidatorFn = (control: AbstractControl) => {
    return control?.value !== ""
      ? this.userService
        .checkUsernameExistence(control.value)
        .pipe(
          map((isTaken) =>
            isTaken
              ? {uniqueUsername: true}
              : null
          )
        )
      : of(null);
  }

  toggleShowVerifyPassword() {
    this.showVerifyPassword = !this.showVerifyPassword;
  }

  toggleShowPassword() {
    this.showPassword = !this.showPassword;
  }
}
