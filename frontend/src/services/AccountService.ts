import { BaseService } from './BaseService';
import type { LoginDto } from '@/types/LoginDto';

export class AccountService extends BaseService {
  async login(email: string, password: string): Promise<LoginDto | null> {
    try {
      const response = await this.axiosInstance.post<LoginDto>(
        'account/login?jwtExpiresInSeconds=5',
        {email, password}
      );
      return response.data;
    } catch (e) {
      console.error('Login failed:', e);
      return null;
    }
  }
}
