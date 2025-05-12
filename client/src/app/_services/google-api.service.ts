// _services/google-api.service.ts
import { inject, Injectable } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { environment } from '../../environments/environment';
import { Observable, from, of } from 'rxjs';

const oAuthConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  strictDiscoveryDocumentValidation: false,
  redirectUri: window.location.origin,
  clientId: '569059783818-28u568ifrharcqulkqa1heo51d8426ms.apps.googleusercontent.com',
  scope: 'openid profile email',
}

@Injectable({
  providedIn: 'root'
})
export class GoogleApiService {
  private readonly oAuthService = inject(OAuthService);

  constructor() {
    this.oAuthService.configure(oAuthConfig);
  }

  configure(): Observable<string | null> {
    return from(this.oAuthService.loadDiscoveryDocument()
      .then(() => {
        return this.oAuthService.tryLoginImplicitFlow();
      })
      .then(() => {
        if(!this.oAuthService.hasValidAccessToken()) {
          this.oAuthService.initLoginFlow();
          return null;
        } else {
          return this.oAuthService.getIdToken();
        }
      })
    );
  }

  logOut(): void {
    this.oAuthService.logOut();
  }
}
