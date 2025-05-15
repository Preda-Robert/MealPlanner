import { inject, Injectable, signal } from '@angular/core';
import { Ingredient } from '../_models/ingredient';
import { IngredientParams } from '../_models/ingredientParams';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { AuthenticationService } from './authentication.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Ingredient[]> | null>(null);
  ingredientCache = new Map();
  user = this.authenticationService.currentUser();
  ingredientParams = signal<IngredientParams>(new IngredientParams());

  getIngredients(searchTerm: string = '') {
    const response = this.ingredientCache.get(Object.values(this.ingredientParams()).join('-'));
    this.ingredientParams().searchTerm = searchTerm;
    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.ingredientParams().pageNumber, this.ingredientParams().pageSize);
    if (this.ingredientParams().searchTerm !== '')
      params = params.append('searchTerm', this.ingredientParams().searchTerm);

    return this.http.get<Ingredient[]>(this.baseUrl + 'ingredients', { observe: 'response', params }).subscribe(
      {
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.ingredientCache.set(Object.values(this.ingredientParams()).join('-'), response);
        }
      });
  }
}
