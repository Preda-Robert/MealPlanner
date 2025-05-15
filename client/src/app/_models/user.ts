export interface User {
  id: number;
  userName: string;
  displayName: string,
  token: string,
  refreshToken: string,
  photoUrl: string,
  emailConfirmed: boolean
  hasDoneSetup: boolean
}
