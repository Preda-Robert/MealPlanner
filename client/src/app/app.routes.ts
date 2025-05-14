import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { adminGuard } from './_guards/admin.guard';
import { LearnMoreComponent } from './learn-more/learn-more.component';
import { AuthComponent } from './auth/auth.component';
import { VerificationComponent } from './verification/verification.component';
import { IngredientsComponent } from './ingredients/ingredients.component';


export const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    children: [
      {path: 'learn-more', component: LearnMoreComponent},
      {path: 'auth', component: AuthComponent},
      {path: 'verify-email', component: VerificationComponent},
      
    ]
  },
  {path: 'ingredients', component: IngredientsComponent },
  {path: 'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];
