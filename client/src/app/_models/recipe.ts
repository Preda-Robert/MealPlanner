  import { Allergy } from "./allergy"
  import { Cookware } from "./cookware"
  import { Photo } from "./photo"
  import { RecipeDifficulty } from "./recipeDifficulty"
  import { RecipeIngredient } from "./recipeIngredient"
  import { RecipeInstruction } from "./recipeInstruction"
  import { ServingType } from "./servingType"

  export interface Recipe {
    id : number
    name : string
    description : string
    cookingTime : number
    photoUrl : string
    rating: number
    Difficulty: RecipeDifficulty
    servingType: ServingType
    dateAdded: Date
    allergies: Allergy[]
    cookware: Cookware[]
    recipeInstructions: RecipeInstruction[]
    recipeIngredients: RecipeIngredient[]

  }
