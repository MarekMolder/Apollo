export interface LoginDto {
  jwt: string;
  refreshToken: string;
  roles: string[];
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  username: string;
}
