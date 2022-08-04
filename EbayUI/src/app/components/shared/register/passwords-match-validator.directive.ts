import { Directive, Input } from '@angular/core';
import {
  AbstractControl,
  FormGroup,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
  ValidatorFn
} from '@angular/forms';
import {RegisterComponent} from "./register.component";

export function passwordsMatchValidator(comp: RegisterComponent): ValidatorFn {
  console.log('a')
  return (control: AbstractControl): ValidationErrors | null => {
    console.log('test');
    console.log('Control Value: ', control.value);
    const match = comp.registerForm.value.password === control.value;
    return !match ? {passwordsMatch: {value: true}} : null;
  };
}

@Directive({
  selector: '[appPasswordsMatchValidator]',
  providers: [{provide: NG_VALIDATORS, useExisting: PasswordsMatchValidatorDirective, multi: true}]
})
export class PasswordsMatchValidatorDirective implements Validator {
  @Input('comp') comp : RegisterComponent;

  validate(control: AbstractControl): ValidationErrors | null {
    return this.comp ? passwordsMatchValidator(this.comp)(control)
      : null;
  }
}
