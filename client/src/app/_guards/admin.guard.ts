import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../_services/authentication.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const authenticationService = inject(AuthenticationService);
  const toastr = inject(ToastrService);

  if(authenticationService.roles().includes('Admin') || authenticationService.roles().includes('Moderator'))
  {
    return true;
  }
  else
  {
  toastr.error('You cannot enter this area');
  return false;
  }
};
