import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-ingredients',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './ingredients.component.html'
})
export class IngredientsComponent implements OnInit {
  ingredients: any[] = [];
  defaultImage = 'assets/placeholder.svg'; // Add this line

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
  this.http.get<any[]>('https://localhost:7110/api/ingredient').subscribe({
    next: (response) => {
      console.log('API Response:', response);
      this.ingredients = response;
    },
    error: (error) => {
      console.error('Full error details:', error);
      if (error.status === 500) {
        console.error('Server error occurred. Check API logs.');
      }
    }
  });
}

  // Add this new method
  getSafeImageUrl(photoUrl: string | null | undefined): string {
    return photoUrl || this.defaultImage;
  }
}