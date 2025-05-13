import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { NgxSpinner, NgxSpinnerComponent } from 'ngx-spinner';
import { GoogleApiService } from './_services/google-api.service';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  private googleApiService = inject(GoogleApiService);

  ngOnInit(){
    this.googleApiService.handleLoginRedirect();
  }
}
