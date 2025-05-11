import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IngredientService, IngredientDTO } from '../services/ingredients.service';
import { NgIf, NgFor } from '@angular/common';

@Component({
  selector: 'app-view-ingredients',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor],
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.css']
})
export class IngredientsComponent implements OnInit {
  ingredients: IngredientDTO[] = [];
  loading = true;
  error: string | null = null;

  constructor(private ingredientService: IngredientService) {}

  ngOnInit(): void {
    this.ingredientService.getIngredients().subscribe({
      next: (data) => {
        this.ingredients = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load ingredients.';
        this.loading = false;
      }
    });
  }
}
