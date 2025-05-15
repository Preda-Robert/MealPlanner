import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { MealPlan } from '../_models/mealPlan';
import { PaginatedResult } from '../_models/pagination';
import { AuthenticationService } from './authentication.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { MealPlanParams } from '../_models/mealPlanParams';

@Injectable({
  providedIn: 'root'
})
export class MealPlanService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<MealPlan[]> | null>(null);
  mealPlanCache = new Map();
  user = this.authenticationService.currentUser();
  mealPlanParams = signal<MealPlanParams>(new MealPlanParams());


  getMealPlans(searchTerm: string = '') {
    const response = this.mealPlanCache.get(Object.values(this.mealPlanParams()).join('-'));
    this.mealPlanParams().searchTerm = searchTerm;
    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.mealPlanParams().pageNumber, this.mealPlanParams().pageSize);
    if(this.mealPlanParams().searchTerm !== '')
      params = params.append('searchTerm', this.mealPlanParams().searchTerm);

    return this.http.get<MealPlan[]>(this.baseUrl + 'mealplans', { observe: 'response', params }).subscribe(
      {
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.mealPlanCache.set(Object.values(this.mealPlanParams()).join('-'), response);
        }
      });
  }
}
