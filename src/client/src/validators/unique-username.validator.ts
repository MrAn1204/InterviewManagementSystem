import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { of, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { UserService } from '../services/user/user.service';

export function uniqueUsernameValidator(userService: UserService): AsyncValidatorFn {
  return (control: AbstractControl): any => {
    if (!control.value) {
      return of(null);
    }
    // Giảm thiểu tần suất gọi API bằng timer (ví dụ 500ms)
    return timer(500).pipe(
      switchMap(() => userService.checkUnique(control.value, null)),
      switchMap(result => {
        return of(result.usernameExists ? { usernameTaken: true } : null);
      })
    );
  };
}
