// _services/google-api.service.ts
import { inject, Injectable, signal } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { environment } from '../../environments/environment';

const oAuthConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  strictDiscoveryDocumentValidation: false,
  redirectUri: window.location.origin,
  clientId: environment.clientId,
  scope: 'openid profile email',
}

export interface  UserProfile{
  info : {
    sub: string,
    name: string,
    email: string,
    picture: string
  }
}

@Injectable({
  providedIn: 'root'
})
export class GoogleApiService {
    private readonly oAuthService = inject(OAuthService);
    idToken = signal<string | null>(null);

  constructor() {
      this.configure();
      //this.handleLoginRedirect();
  }

  configure(): void {
    this.oAuthService.configure(oAuthConfig);
  }

  handleLoginRedirect(): void {
    this.oAuthService.loadDiscoveryDocument().then(() => {
      this.oAuthService.tryLoginImplicitFlow().then(() => {
        if (this.oAuthService.hasValidAccessToken()) {
          const token = this.oAuthService.getIdToken();
          this.idToken.set(token);
          //console.log('OAuth successful, idToken:', token);
        }
      });
    });
  }

  loginWithGoogle(): void {
    this.oAuthService.initLoginFlow();
  }

  logOut(): void {
    this.oAuthService.logOut();
  }
}
