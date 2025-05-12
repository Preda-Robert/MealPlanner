import { User } from "./user";

export class UserParams
{
  minAge = 18;
  maxAge = 100;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user: User | null)
  {
  }
}
