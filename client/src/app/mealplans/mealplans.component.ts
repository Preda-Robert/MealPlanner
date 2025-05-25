import { Component, OnInit, inject } from '@angular/core';
import { MealPlanService } from '../_services/meal-plan.service';
import { RecipeService } from '../_services/recipe.service';
import { MealPlan } from '../_models/mealPlan';
import { Recipe } from '../_models/recipe';
import { addDays, startOfWeek, endOfWeek } from 'date-fns';
import { CommonModule } from '@angular/common';
import { NgIf, NgFor, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-mealplans',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, DatePipe, FormsModule],
  templateUrl: './mealplans.component.html'
})
export class MealPlansComponent implements OnInit {
  currentStart!: Date;
  currentEnd!: Date;
  mealPlan?: MealPlan;
  error?: string;
  showCreateMealPlanForm = false;
  isCreating = false;
  
  // Form data
  newMealPlan = {
    name: '',
    description: '',
    startDate: new Date(),
    endDate: new Date(),
    userId: 0,
    mealPlanRecipes: [] as any[]
  };
  
  // Recipe selection
  availableRecipes: Recipe[] = [];
  selectedRecipes: Recipe[] = [];
  recipeSearchTerm = '';

  private mealPlanService = inject(MealPlanService);
  private recipeService = inject(RecipeService);
  private authService = inject(AuthenticationService);

  ngOnInit(): void {
    this.setCurrentWeek();
    this.loadMealPlan();
    this.loadAvailableRecipes();
  }

  setCurrentWeek(date = new Date()) {
    this.currentStart = startOfWeek(date, { weekStartsOn: 1 }); // Monday
    this.currentEnd = endOfWeek(date, { weekStartsOn: 1 });
    
    // Update form dates when week changes
    this.newMealPlan.startDate = this.currentStart;
    this.newMealPlan.endDate = this.currentEnd;
    this.newMealPlan.name = `Meal Plan for ${this.currentStart.toLocaleDateString()} - ${this.currentEnd.toLocaleDateString()}`;
  }

  loadMealPlan() {
    this.mealPlanService.getByDateRange(this.currentStart, this.currentEnd).subscribe({
      next: plan => {
        this.mealPlan = plan;
        this.error = undefined;
      },
      error: err => {
        this.mealPlan = undefined;
        this.error = err.error || 'Meal plan not found.';
      }
    });
  }

  loadAvailableRecipes() {
    // Load recipes for selection
    this.recipeService.getRecipes();
    // Subscribe to recipe changes
    this.availableRecipes = this.recipeService.paginatedResult()?.items || [];
  }

  searchRecipes() {
    if (this.recipeSearchTerm.trim()) {
      this.recipeService.getRecipes(this.recipeSearchTerm);
    } else {
      this.recipeService.getRecipes();
    }
    this.availableRecipes = this.recipeService.paginatedResult()?.items || [];
  }

  toggleRecipeSelection(recipe: Recipe, event: any) {
    if (event.target.checked) {
      if (!this.selectedRecipes.find(r => r.id === recipe.id)) {
        this.selectedRecipes.push(recipe);
      }
    } else {
      this.selectedRecipes = this.selectedRecipes.filter(r => r.id !== recipe.id);
    }
  }

  removeRecipeFromSelection(recipe: Recipe) {
    this.selectedRecipes = this.selectedRecipes.filter(r => r.id !== recipe.id);
  }

  createMealPlan() {
    this.isCreating = true;
    
    // Prepare meal plan data
    const currentUser = this.authService.currentUser();
    this.newMealPlan.userId = currentUser?.id || 0;
    
    // Map selected recipes to meal plan recipes
    this.newMealPlan.mealPlanRecipes = this.selectedRecipes.map(recipe => ({
      recipeId: recipe.id,
      servingTypeId: recipe.servingType.id,
      recipe: {
        id: recipe.id,
        name: recipe.name,
        description: recipe.description
      },
      servingType: recipe.servingType
    }));

    this.mealPlanService.createMealPlan(this.newMealPlan).subscribe({
      next: (createdPlan) => {
        this.mealPlan = createdPlan;
        this.showCreateMealPlanForm = false;
        this.resetForm();
        this.isCreating = false;
        this.error = undefined;
      },
      error: (err) => {
        this.error = err.error || 'Failed to create meal plan';
        this.isCreating = false;
      }
    });
  }

  cancelCreateMealPlan() {
    this.showCreateMealPlanForm = false;
    this.resetForm();
  }

  resetForm() {
    this.newMealPlan = {
      name: `Meal Plan for ${this.currentStart.toLocaleDateString()} - ${this.currentEnd.toLocaleDateString()}`,
      description: '',
      startDate: this.currentStart,
      endDate: this.currentEnd,
      userId: 0,
      mealPlanRecipes: []
    };
    this.selectedRecipes = [];
    this.recipeSearchTerm = '';
  }

  editMealPlan() {
    // TODO: Implement edit functionality
    console.log('Edit meal plan functionality to be implemented');
  }

  deleteMealPlan() {
    if (confirm('Are you sure you want to delete this meal plan?')) {
      // TODO: Implement delete functionality
      console.log('Delete meal plan functionality to be implemented');
    }
  }

  prevWeek() {
    this.setCurrentWeek(addDays(this.currentStart, -7));
    this.loadMealPlan();
  }

  nextWeek() {
    this.setCurrentWeek(addDays(this.currentStart, 7));
    this.loadMealPlan();
  }
}