import { Allergy } from "./allergy"
import { DietType } from "./dietType"
import { ServingType } from "./servingType"

export interface DietaryPreference {
  id : number
  userId : number
  dietType : DietType
  servingType : ServingType
  allergies : Allergy[]

}
