import { HttpInterceptorFn } from '@angular/common/http';
import { AuthenticationService } from '../_services/authentication.service';
import { inject } from '@angular/core';
// this is a centralized way of sending the auth token to the server
// just have this in one place
export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authenticationService = inject(AuthenticationService);

  if(authenticationService.currentUser())
  {
    req = req.clone({ // need to clone as req is immutable
      setHeaders: {
        Authorization: `Bearer ${authenticationService.currentUser()?.token}`
      }
    });
  }
  return next(req);
};
