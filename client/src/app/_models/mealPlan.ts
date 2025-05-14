import { MealPlanRecipe } from "./mealPlanRecipe";
import { Photo } from "./photo";

export interface MealPlan {
  id : number;
  name : string;
  description : string;
  photo : Photo;
  mealPlanRecipes : MealPlanRecipe[]
}
