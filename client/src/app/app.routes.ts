import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { adminGuard } from './_guards/admin.guard';
import { LearnMoreComponent } from './learn-more/learn-more.component';
import { AuthComponent } from './auth/auth.component';
import { VerificationComponent } from './verification/verification.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { IngredientsComponent } from './ingredients/ingredients.component';
import { RecipesComponent } from './recipes/recipes.component';
import { CookwaresComponent } from './cookwares/cookwares.component';
import { SetupSelectionComponent } from './setup-selection/setup-selection.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    children: [
      {path: 'learn-more', component: LearnMoreComponent},
      {path: 'auth', component: AuthComponent},
      {path: 'verify-email', component: VerificationComponent},
      {path: 'ingredients', component: IngredientsComponent},
      {path: 'recipes', component: RecipesComponent},
      {path: 'cookwares', component: CookwaresComponent},
      {path: 'member-details', component: MemberDetailComponent, resolve: {member: memberDetailedResolver}, canActivate: [authGuard]},
      {path: 'setup-selection', component: SetupSelectionComponent}
    ]
  },
  {path: 'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];
