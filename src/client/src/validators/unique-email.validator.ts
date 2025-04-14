import { AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { of, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { UserService } from '../services/user/user.service';

export function uniqueEmailValidator(userService: UserService): AsyncValidatorFn {
  return (control: AbstractControl): any => {
    if (!control.value) {
      return of(null);
    }
    return timer(500).pipe(
      switchMap(() => userService.checkUnique(null, control.value)),
      switchMap(result => {
        return of(result.emailExists ? { emailTaken: true } : null);
      })
    );
  };
}
