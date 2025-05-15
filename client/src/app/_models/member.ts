import { DietaryPreference } from "./dietaryPreference"
import { MealPlan } from "./mealPlan"
import { Recipe } from "./recipe"

export interface Member {
  id: number
  userName: string
  displayname: string
  photoUrl: string
  dietaryPreference : DietaryPreference
  createdRecipes : Recipe[]
  mealPlans : MealPlan[]
}


