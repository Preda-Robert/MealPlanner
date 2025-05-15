import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { tap } from 'rxjs';
import { DietType } from '../_models/dietType';

@Injectable({
  providedIn: 'root'
})
export class DietTypeService {

  http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  dietTypes: DietType[] = [];

  getDietTypes() {
    return this.http.get<DietType[]>(this.baseUrl + 'diettypes').pipe(
      tap({
        next: (response: DietType[]) => {
          if (!response || response.length === 0) {
            console.error('No diet types found');
          } else {
            this.dietTypes = response;
          }
        },
        error: (error: any) => {
          console.error('Error fetching diet types:', error);
        }
      })
    );
  }
}
