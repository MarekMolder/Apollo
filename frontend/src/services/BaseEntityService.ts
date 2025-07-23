import type { IResultObject } from "@/types/IResultObject";
import { BaseService } from "@/services/BaseService";
import { useUserDataStore } from "@/stores/userDataStore";
import type {IDomainId} from "@/domain/IDomainId.ts";

export abstract class BaseEntityService<TEntity extends IDomainId> extends BaseService {
  private store = useUserDataStore();
  protected entityUrl: string;

  constructor(basePath: string) {
    super();
    this.entityUrl = basePath;
  }

  async getAllAsync(): Promise<IResultObject<TEntity[]>> {
    try {
      const response = await this.axiosInstance.get<TEntity[]>(this.entityUrl);
      if (response.status <= 300) {
        return { data: response.data };
      }

      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error) {
      return {
        errors: [JSON.stringify(error)],
      };
    }
  }

  async addAsync(entity: TEntity): Promise<IResultObject<TEntity>> {
    try {
      const response = await this.axiosInstance.post<TEntity>(this.entityUrl, entity);
      if (response.status <= 300) {
        return { data: response.data };
      }

      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error) {
      return {
        errors: [JSON.stringify(error)],
      };
    }
  }

  async updateAsync(entity: TEntity): Promise<IResultObject<TEntity>> {
    try {
      const response = await this.axiosInstance.put<TEntity>(`${this.entityUrl}/${entity.id}`, entity);
      if (response.status < 300) {
        return { data: response.data };
      }

      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error: any) {
      return {
        errors: [error?.response?.data?.message || error.message || "Unknown error"],
      };
    }
  }

  async removeAsync(id: string): Promise<IResultObject<null>> {
    try {
      const response = await this.axiosInstance.delete(`${this.entityUrl}/${id}`);
      if (response.status < 300) {
        return { data: null };
      }

      return {
        errors: [`${response.status} ${response.statusText}`],
      };
    } catch (error: any) {
      return {
        errors: [error?.response?.data?.message || error.message || "Unknown error"],
      };
    }
  }

}
