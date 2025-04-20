// src/app/validators/dob.validator.ts
import { AbstractControl, ValidationErrors } from '@angular/forms';

export function dobValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (!value) return null;

  const inputDate = new Date(value);
  const today = new Date();

  if (inputDate > today) {
    return { futureDate: true };
  }

  return null;
}
