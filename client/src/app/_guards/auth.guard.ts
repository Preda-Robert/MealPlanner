import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../_services/authentication.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authenticationService = inject(AuthenticationService);
  const toastr = inject(ToastrService);

  if(authenticationService.currentUser())
  {
    return true;
  }
  else
  {
    toastr.error('You shall not pass! (You are not logged in)');
    return false;
  }
};
