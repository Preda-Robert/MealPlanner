import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-ingredients',
  standalone: true, // ✅ Important for standalone setup
  imports: [CommonModule, HttpClientModule], // ✅ Required
  templateUrl: './ingredients.component.html'
})
export class IngredientsComponent implements OnInit {
  ingredients: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
  this.http.get<any[]>('https://localhost:7110/api/ingredient').subscribe({
    next: (response) => {
      console.log('API Response:', response); // Debug: Check if all items are here
      this.ingredients = response;
    },
    error: (error) => console.error('Error fetching ingredients', error)
  });
}
}
