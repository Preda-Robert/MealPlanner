import { PaginationParams } from "./paginationParams";

export interface CookwareParams extends PaginationParams {
  searchTerm: string;
}
