import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NgxSpinner } from 'ngx-spinner';
import { IngredientsComponent } from './ingredients/ingredients.component';

export const routes: Routes = [
  { path: 'ingredients', component: IngredientsComponent },
  { path: '', redirectTo: 'ingredients', pathMatch: 'full' } // Optional default route
];