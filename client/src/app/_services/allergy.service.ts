import { inject, Injectable, signal } from '@angular/core';
import { Allergy } from '../_models/allergy';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { AllergyParams } from '../_models/allergyParams';
import { PaginatedResult } from '../_models/pagination';
import { AuthenticationService } from './authentication.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class AllergyService {
  private http = inject(HttpClient);
  private authenticationService = inject(AuthenticationService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Allergy[]> | null>(null);
  allergyCache = new Map();
  user = this.authenticationService.currentUser();
  allergyParams = signal<AllergyParams>(new AllergyParams());

  getAllergies() {
    const response = this.allergyCache.get(Object.values(this.allergyParams()).join('-'));

    if (response !== undefined) {
      return setPaginatedResponse(response, this.paginatedResult);
    }

    let params = setPaginationHeaders(this.allergyParams().pageNumber, this.allergyParams().pageSize);
    params = params.append('searchTerm', this.allergyParams().searchTerm);

    return this.http.get<Allergy[]>(this.baseUrl + 'allergies', { observe: 'response', params }).subscribe(
      {
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.allergyCache.set(Object.values(this.allergyParams()).join('-'), response);
        }
      });
  }
}
