import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseService} from "@/services/BaseService.ts";
import type {LoginDto} from "@/types/LoginDto.ts";
import type { UserFieldUpdate } from '@/types/UserFieldUpdate.ts';
import {useUserDataStore} from "@/stores/userDataStore.ts";
import { jwtDecode } from 'jwt-decode';

interface DecodedJwt {
  email?: string;
  username?: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  role?: string;
  [key: string]: unknown;
}

export class IdentityService extends BaseService {

  async login(email: string, password: string): Promise<IResultObject<LoginDto>> {
    const url = "account/login";
    try {
      const loginData = {
        email,
        password,
      };
      const response = await this.axiosInstance.post<LoginDto>(url, loginData);

      console.log('login response', response);
      if (response.status <= 300) {
        const store = useUserDataStore();

        store.jwt = response.data.jwt;
        store.refreshToken = response.data.refreshToken;
        store.email = response.data.email;
        store.firstName = response.data.firstName;
        store.lastName = response.data.lastName;
        store.userId = response.data.userId;
        store.username = response.data.username;
        store.roles = response.data.roles ?? [];

        return { data: response.data };
      }

      return {
        errors: [(response.status.toString() + " " + response.statusText).trim()],
      };
    } catch (error) {
      console.log('error: ', (error as Error).message);
      return {
        errors: [JSON.stringify(error)],
      };
    }
  }

  async updateUserFields(update: UserFieldUpdate): Promise<IResultObject<void>> {
    const url = `account/updateuserfields`;

    try {
      const response = await this.axiosInstance.put(url, update);
      if (response.status === 204) {
        const store = useUserDataStore();
        Object.assign(store, update);
        return { data: undefined };
      }

      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error) {
      return {
        errors: [(error as Error).message],
      };
    }
  }

  async changePassword(data: {
    email: string;
    currentPassword: string;
    newPassword: string;
  }): Promise<IResultObject<void>> {
    const url = 'account/changepassword';
    try {
      const response = await this.axiosInstance.post(url, data);
      if (response.status === 204) {
        return { data: undefined };
      }
      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error) {
      return {
        errors: [(error as Error).message],
      };
    }
  }

  async getAllUsers(): Promise<{ id: string, username: string, email: string, firstName: string, lastName: string }[]> {
    const response = await this.axiosInstance.get('/users/all');
    return response.data;
  }

  async getUserById(id: string): Promise<{
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    userName: string;
  }> {
    const response = await this.axiosInstance.get(`/users/${id}`);
    return response.data;
  }

  async register(data: {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
  }): Promise<IResultObject<LoginDto>> {
    const response = await this.axiosInstance.post<LoginDto>('account/register', data);
    return { data: response.data };
  }

}
