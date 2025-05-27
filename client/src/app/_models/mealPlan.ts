// Updated _models/mealPlan.ts
import { MealPlanRecipe } from "./mealPlanRecipe";
import { Photo } from "./photo";

export interface MealPlan {
  id: number;
  name: string;
  description: string;
  photo?: Photo;
  startDate: Date | string;
  endDate: Date | string;
  userId: number;
  mealPlanRecipes: MealPlanRecipe[];
}