import { Allergy } from "./allergy"
import { DietType } from "./dietType"
import { ServingType } from "./servingType"

export interface DietaryPreference {
  dietType : DietType
  servingType : ServingType
  allergies : Allergy

}
