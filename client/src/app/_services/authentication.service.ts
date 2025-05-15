import { effect, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject, signal, computed } from '@angular/core';
import { User } from '../_models/user';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../environments/environment.development';
import { FavoritesService } from './favorites.service';
import { throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CookieService } from 'ngx-cookie-service';
import { EmailVerification } from '../_models/emailVerification';
import { ResetPasswordRequest } from '../_models/password-reset';
import { GoogleApiService } from './google-api.service';
import { RegisterResponse } from '../_models/registerResponse';
import { DietaryPreference } from '../_models/dietaryPreference';
import { SaveDietPreference } from '../_models/saveDietPreference';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private http = inject(HttpClient);
  private favoritesService = inject(FavoritesService);
  private toastr = inject(ToastrService);
  private googleapiService = inject(GoogleApiService);
  private googleAuthenticated : boolean = false;

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);
  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const role = JSON.parse(atob(user.token.split('.')[1])).role;
      return Array.isArray(role) ? role : [role];
    }
    return [null];
  });

  constructor() {
    effect(() => {
      const idToken = this.googleapiService.idToken();
      if(!this.googleAuthenticated && idToken) {
        this.googleLogin(idToken).subscribe({
          next: user => {
            if (user) {
              this.googleAuthenticated = true;
            }
          },
          error: error => {
            //.error('Google login error:', error);
          }
        });
      }
      else if (idToken === null) {
        this.googleAuthenticated = false;
      }
    });
  }


  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'authentication/login', model).pipe(
      map(user => {
        if (user) {
          if(user.emailConfirmed !== false)
          {
            this.setCurrentUser(user);
            localStorage.setItem('user', JSON.stringify(user));
          }
          return user;
        }
        return undefined;
      },
      catchError(error => {
        if (error.status !== 400) {
          this.toastr.error('Login failed');
        }
        return throwError(() => 'Login failed. Please try again.');
      })
      )
    );
  }

  verifyEmailCode(verificationData: EmailVerification) {
    console.log(verificationData);
    return this.http.post(this.baseUrl + 'authentication/verify-email', verificationData).pipe(
      map(() => {
        return true;
      }),
      catchError(error => {
          this.toastr.error('Email verification failed');
        return throwError(() => 'Email verification failed. Please try again.');
      })
    );
  }

  resendVerificationCode(userId: number) {
    const resendData = { userId: userId };

    return this.http.post(this.baseUrl + 'authentication/resend-verification-code', resendData).pipe(
      map(() => {
        return true;
      }),
      catchError(error => {
        console.error('Resend error:', error);
        return throwError(() => error);
      })
    );
  }

  googleLogin(idToken: string) {
    //console.log('googleLogin method called with idToken:', idToken);
    //console.log('Sending Google login request to:', this.baseUrl + 'authentication/google-auth');
    const token = { IdToken: idToken };
    //.log('Request payload:', token);
    //console.log('API baseUrl:', this.baseUrl);

    return this.http.post<User>(this.baseUrl + 'authentication/google-auth', token).pipe(
      map(user => {
        //console.log('HTTP response received:', user);
        if (user) {
          this.setCurrentUser(user);
          localStorage.setItem('user', JSON.stringify(user));
          this.toastr.success('Logged in with Google successfully');
          return user;
        }
        return null;
      }),
      catchError(error => {
        console.error('HTTP request error:', {
          status: error.status,
          statusText: error.statusText,
          message: error.message,
          error: error.error
        });
        this.toastr.error('Google login failed: ' + (error.message || 'Unknown error'));
        return throwError(() => 'Google login failed. Please try again.');
      })
    );
  }

  register(model: RegisterResponse) {
    console.log('Sending registration request to:', this.baseUrl + 'authentication/register');
    console.log('Registration data:', model);

    return this.http.post<RegisterResponse>(this.baseUrl + 'authentication/register', model).pipe(
      map(response => {
        //console.log('Raw registration response:', response);

        return response;
      }),
      catchError(error => {
        //console.error('Registration error in service:', error);

        if (error.status === 400) {
          if (typeof error.error === 'string') {
            this.toastr.error(error.error);
            return throwError(() => error.error);
          }
          else if (error.error && Array.isArray(error.error)) {
            return throwError(() => error.error);
          }
        }

        this.toastr.error('Registration failed');
        return throwError(() => 'An unexpected error occurred. Please try again later.');
      })
    );
  }

  doneSelection(dietpreferences : SaveDietPreference) {
    this.logout();
    let user = JSON.parse(localStorage.getItem('user') || '{}');
    if (user && user.id) {
      this.http.get<User>(this.baseUrl + 'user/' + user.id).subscribe({
        next: (response) => {
          if (response) {
            user = response;
            user.hasDoneSetup = true;
            this.setCurrentUser(user);
            localStorage.setItem('user', JSON.stringify(user));
          }
        }
      });
    }
    user = JSON.parse(localStorage.getItem('user') || '{}');
    this.currentUser.set(user);
    this.toastr.info('Please log in again to update your profile');
  }


  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.favoritesService.getLikeIds();
  }

  refreshUserData() {
    return this.http.get<User>(this.baseUrl + 'user').pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
          return user;
        }
        return null;
      }),
      catchError(error => {
        this.toastr.error('Failed to refresh user data');
        return throwError(() => error);
      })
    );
  }

  forgotPassword(email: string) {
    return this.http.post(this.baseUrl + 'account/forgot-password', { email }).pipe(
      map(() => {
        this.toastr.info('If your email is registered, you will receive a password reset code');
        return true;
      }),
      catchError(error => {
        console.error('Forgot password error:', error);
        this.toastr.error('Failed to process forgot password request');
        return throwError(() => 'Failed to process forgot password request');
      })
    );
  }

  resetPassword(resetData: ResetPasswordRequest) {
    return this.http.post(this.baseUrl + 'account/reset-password', resetData).pipe(
      map(() => {
        this.toastr.success('Your password has been reset successfully');
        return true;
      }),
      catchError(error => {
        console.error('Reset password error:', error);
        this.toastr.error(error.error || 'Failed to reset password');
        return throwError(() => error.error || 'Failed to reset password');
      })
    );
  }

  logout() {
    this.googleapiService.logOut();
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
