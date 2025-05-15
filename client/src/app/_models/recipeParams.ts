import { PaginationParams } from "./paginationParams";

export class RecipeParams extends PaginationParams {
  searchTerm: string = '';
  userId: number = 0;
}
