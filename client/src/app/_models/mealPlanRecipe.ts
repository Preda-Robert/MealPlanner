import { ServingType } from "./servingType";

export interface MealPlanRecipe {
  id : number;
  servingType : ServingType;
  recipe : ServingType;
}
