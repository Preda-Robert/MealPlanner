import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

export interface IngredientDTO {
  name: string;
  category: { name: string };
  isAllergen: boolean;
  measurementType: string;
  calories: number;
  photo?: { url: string };
}

@Injectable({ providedIn: 'root' })
export class IngredientService {
  private apiUrl = `${environment.apiUrl}ingredients`;

  constructor(private http: HttpClient) {}

  getIngredients(): Observable<IngredientDTO[]> {
    return this.http.get<IngredientDTO[]>(this.apiUrl);
  }
}
