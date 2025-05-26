import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, Signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AuthenticationService } from './authentication.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { RecipeParams } from '../_models/recipeParams';
import { MealPlan } from '../_models/mealPlan';
import { Recipe } from '../_models/recipe';
import { AllergyService } from './allergy.service';
import { CookwareService } from './cookware.service';
import { MealPlanService } from './meal-plan.service';
import { RecipeService } from './recipe.service';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  private recipeService = inject(RecipeService);
  private mealPlanService = inject(MealPlanService);
  private cookwareService = inject(CookwareService);
  private allergyService = inject(AllergyService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();
  user = computed(() => this.authenticationService.currentUser());
  member = signal<Member | null>(null);
  userParams = signal<UserParams>(new UserParams(this.user()));

  resetUserParams() {
    this.userParams.set(new UserParams(this.user()));
  }

  loadMember() {
    return this.http.get<Member>(this.baseUrl + 'users/' + this.user()?.userName).pipe(
      tap((member: Member) => {
        console.log('Member loaded:', member);
        this.member.set(member);
      })
    );
  }

  getMembers() {
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));

    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);
    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).subscribe(
      {
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.memberCache.set(Object.values(this.userParams()).join('-'), response);
        }
      });
  }

  getPaginatedRecipes() {
    this.recipeService.getRecipes();
  }

  getPaginatedMealPlans() {
    this.mealPlanService.getMealPlans();
  }

  getPaginatedCookware() {
    this.cookwareService.getCookwares();
  }

  getPaginatedAllergies() {
    this.allergyService.getAllergies();
  }

  getMember(username: string) {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);

    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(x => x.username === member.username ? member : x)); // find the updated member and update it in the signal members
      // })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe
      (
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if(m.photos.includes(photo))
      //     {
      //       m.photoUrl = photo.url;
      //     }
      //     return m;
      //   }));
      // })
    );
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if(m.photos.includes(photo))
      //       m.photos = m.photos.filter(p => p.id !== photo.id);
      //     return m;
      //   }));
      // })
    );
  }

}
