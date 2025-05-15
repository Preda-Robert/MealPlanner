import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { RecipeService } from '../_services/recipe.service';
import { NgClass } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ViewRecipeComponent } from "../view-recipe-details/view-recipe-details.component";
import { Recipe } from '../_models/recipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-recipes',
  imports: [TabsModule, NgClass, PaginationModule, FormsModule, ViewRecipeComponent],
  templateUrl: './recipes.component.html',
  styleUrl: './recipes.component.css'
})
export class RecipesComponent implements OnInit {
  searchQuery: string = '';
  recipeService = inject(RecipeService);
  recipes : Recipe[] | undefined = [];
  router = inject(Router);

  onSearch() {
    this.getPaginatedRecipes();
  }

  ngOnInit(): void {
    this.recipeService.getRecipes();
    this.recipes = this.recipeService.paginatedResult()?.items;
  }

  onRecipePageChanged(event: any) {
    if (this.recipeService.recipeParams().pageNumber !== event.page) {
      this.recipeService.recipeParams().pageNumber = event.page;
      this.loadRecipes();
    }
  }

  loadRecipes() {
    this.getPaginatedRecipes();
  }

  viewRecipeDetails(recipeId : number)
  {
    for(let recipe of this.recipes!)
    {
      if(recipe.id == recipeId)
      {
        this.router.navigate(['/recipe', recipeId], { state: { recipe: recipe } });
        break;
      }
    }
  }

  getPaginatedRecipes() {
    this.recipeService.getRecipes(this.searchQuery);
    this.recipes = this.recipeService.paginatedResult()?.items;
  }


}
