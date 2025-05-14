import { PaginationParams } from "./paginationParams";

export interface IngredientParams extends PaginationParams {
  searchTerm: string;
}
