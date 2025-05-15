import { Allergy } from "./allergy";
import { IngredientCategory } from "./ingredientCategory";
import { MeasurementType } from "./measurementType";
import { Photo } from "./photo";

export interface Ingredient {
  id : number;
  name : string;
  category : IngredientCategory;
  isAllergen : boolean;
  allergy : Allergy
  MeasurementType : MeasurementType;
  calories : number;
  photoUrl : string;
}
