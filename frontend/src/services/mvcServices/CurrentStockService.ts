import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IInventory} from "@/domain/logic/IInventory.ts";
import type {ICurrentStock} from "@/domain/logic/ICurrentStock.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {ICurrentStockEnriched} from "@/domain/logic/ICurrentStockEnriched.ts";

export class CurrentStockService extends BaseEntityService<ICurrentStock> {
  constructor() {
    super('currentstocks');
  }

  async getByStorageRoomId(id: string): Promise<IResultObject<ICurrentStockEnriched[]>> {
    const response = await this.axiosInstance.get(`/currentstocks/bystorageroom/${id}`);
    return { data: response.data };
  }

  async getLowestStockProducts(): Promise<{ productId: string; productName: string; quantity: number }[]> {
    const response = await this.axiosInstance.get('/currentstocks/lowestStockProducts');
    return response.data;
  }

  async getTotalWorth(inventoryId?: string): Promise<number> {
    const url = inventoryId
      ? `/currentstocks/totalWorth?inventoryId=${inventoryId}`
      : `/currentstocks/totalWorth`;

    const response = await this.axiosInstance.get(url);
    return response.data;
  }


}
