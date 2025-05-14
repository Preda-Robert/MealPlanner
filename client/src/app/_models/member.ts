import { DietaryPreference } from "./dietaryPreference"
import { MealPlan } from "./mealPlan"
import { Photo } from "./photo"
import { Recipe } from "./recipe"

export interface Member {
  id: number
  username: string
  displayname: string
  photo: Photo
  dietaryPreference : DietaryPreference
  createdRecipes : Recipe[]
  mealPlans : MealPlan[]
}


