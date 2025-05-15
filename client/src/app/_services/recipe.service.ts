import { inject, Injectable, signal } from '@angular/core';
import { RecipeParams } from '../_models/recipeParams';
import { Recipe } from '../_models/recipe';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { PaginatedResult } from '../_models/pagination';
import { AuthenticationService } from './authentication.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { Allergy } from '../_models/allergy';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Recipe[]> | null>(null);
  recipeCache = new Map();
  user = this.authenticationService.currentUser();
  recipeParams = signal<RecipeParams>(new RecipeParams());
  allergyIds = signal<number[]>([]);

  getRecipes(searchTerm : string = '') {
    const response = this.recipeCache.get(Object.values(this.recipeParams()).join('-'));
    this.recipeParams().searchTerm = searchTerm;
    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.recipeParams().pageNumber, this.recipeParams().pageSize);
    if (this.recipeParams().searchTerm !== '')
      params = params.append('searchTerm', this.recipeParams().searchTerm);
    params = params.append('userId', this.recipeParams().userId);

    console.log(params);

    return this.http.get<Recipe[]>(this.baseUrl + 'recipes', { observe: 'response', params }).subscribe(
      {
        next: response => {

          setPaginatedResponse(response, this.paginatedResult);
          this.recipeCache.set(Object.values(this.recipeParams()).join('-'), response);
        }
      });
  }
}
