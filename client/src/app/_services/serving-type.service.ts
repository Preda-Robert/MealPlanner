import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ServingType } from '../_models/servingType';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServingTypeService {

  http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  servingTypes: ServingType[] = [];

  getServingTypes() {
    return this.http.get<ServingType[]>(this.baseUrl + 'servingtypes').pipe(
      tap({
        next: (response: ServingType[]) => {
          if (!response || response.length === 0) {
            console.error('No serving types found');
          } else {
            this.servingTypes = response;
          }
        },
        error: (error: any) => {
          console.error('Error fetching serving types:', error);
        }
      })
    );
  }
}
