export interface User {
  id: number;
  username: string;
  displayName: string,
  token: string,
  refreshToken: string,
  photoUrl: string,
  emailConfirmed: boolean
}
