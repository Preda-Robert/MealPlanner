import { PaginationParams } from "./paginationParams";

export interface RecipeParams extends PaginationParams {
  searchTerm: string;
  allergyIds: number[];
}
