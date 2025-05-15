import { Component, inject, ViewChild } from '@angular/core';
import { IngredientService } from '../_services/ingredient.service';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { NgClass } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ingredients',
  imports: [NgClass, PaginationModule, FormsModule],
  templateUrl: './ingredients.component.html',
  styleUrl: './ingredients.component.css'
})
export class IngredientsComponent {
  searchQuery: string = '';
  ingredientService = inject(IngredientService);


  onSearch() {
    this.getPaginatedRecipes();
  }

  ngOnInit(): void {
    this.ingredientService.getIngredients();
  }

  onIngredientPageChanged(event: any) {
    if (this.ingredientService.ingredientParams().pageNumber !== event.page) {
      this.ingredientService.ingredientParams().pageNumber = event.page;
      this.loadRecipes();
    }
  }

  loadRecipes() {
    this.getPaginatedRecipes();
  }

  getPaginatedRecipes() {
    this.ingredientService.getIngredients(this.searchQuery);
  }

}
