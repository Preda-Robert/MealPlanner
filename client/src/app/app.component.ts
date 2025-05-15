import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { NgxSpinner, NgxSpinnerComponent } from 'ngx-spinner';
import { GoogleApiService } from './_services/google-api.service';
import { AuthenticationService } from './_services/authentication.service';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  router = inject(Router);
  private googleApiService = inject(GoogleApiService);
  private authenticationService = inject(AuthenticationService);

  ngOnInit(){
    if(localStorage.getItem('user') !== null)
    {
      const currentUser = JSON.parse(localStorage.getItem('user')!);
      this.authenticationService.currentUser.set(currentUser);
      if(this.authenticationService.currentUser()?.hasDoneSetup === false)
      {
        this.router.navigate(['/setup-selection']);
      }
    }
  }
}
