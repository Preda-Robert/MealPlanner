export interface EmailVerification {
  id: number;
  code: string;
}

export interface ResendVerificationCode {
  userId: number;
}
