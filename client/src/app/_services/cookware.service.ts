import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { CookwareParams } from '../_models/cookwareParams';
import { PaginatedResult } from '../_models/pagination';
import { AuthenticationService } from './authentication.service';
import { Cookware } from '../_models/cookware';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class CookwareService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Cookware[]> | null>(null);
  cookwareCache = new Map();
  user = this.authenticationService.currentUser();
  cookwareParams = signal<CookwareParams>(new CookwareParams());


  getCookwares(searchTerm: string = '') {
    const response = this.cookwareCache.get(Object.values(this.cookwareParams()).join('-'));
    this.cookwareParams().searchTerm = searchTerm;
    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.cookwareParams().pageNumber, this.cookwareParams().pageSize);
    if (this.cookwareParams().searchTerm !== '')
      params = params.append('searchTerm', this.cookwareParams().searchTerm);

    return this.http.get<Cookware[]>(this.baseUrl + 'cookwares', { observe: 'response', params }).subscribe(
      {
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.cookwareCache.set(Object.values(this.cookwareParams()).join('-'), response);
        }
      });
  }
}
