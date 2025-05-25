import { Recipe } from "./recipe";
import { ServingType } from "./servingType";

export interface MealPlanRecipe {
  id : number;
  servingType : ServingType;
  recipe : Recipe;
}
