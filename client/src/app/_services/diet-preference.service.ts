import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DietaryPreference } from '../_models/dietaryPreference';
import { tap } from 'rxjs';
import { SaveDietPreference } from '../_models/saveDietPreference';

@Injectable({
  providedIn: 'root'
})
export class DietPreferenceService {

  http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  dietPreferences: DietaryPreference[] = [];

  getDietPreferences() {
    return this.http.get<DietaryPreference[]>(this.baseUrl + 'dietpreferences').pipe(
      tap({
        next: (response: DietaryPreference[]) => {
          if (!response || response.length === 0) {
            console.error('No diet preferences found');
          } else {
            this.dietPreferences = response;
          }
        },
        error: (error: any) => {
          console.error('Error fetching diet preferences:', error);
        }
      })
    );
  }

  saveDietPreferences(dietPreferences: SaveDietPreference) {
    console.log('Saving diet preferences:', dietPreferences);
    return this.http.post<SaveDietPreference>(this.baseUrl + 'dietarypreference/save', dietPreferences).pipe(
      tap({
        next: (response: SaveDietPreference) => {
          if (!response) {
            console.error('No diet preferences found');
          }
        },
        error: (error: any) => {
          console.error('Error saving diet preferences:', error);
        }
      })
    );
  }
}
