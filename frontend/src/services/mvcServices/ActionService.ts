import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IAction} from "@/domain/logic/IAction.ts";
import type {IActionEnriched} from "@/domain/logic/IActionEnriched.ts";

export class ActionService extends BaseEntityService<IAction> {
  constructor() {
    super('actions');
  }

  async patchStatus(id: string, status: 'Accepted' | 'Declined') {
    return await this.axiosInstance.patch(`${this.entityUrl}/${id}/status`, { status })
      .then(res => res.data as { message: string });
  }

  async getEnrichedActions(): Promise<IResultObject<IActionEnriched[]>> {
    const response = await this.axiosInstance.get('/actions/enrichedAction');
    return {
      data: response.data as IActionEnriched[],
      errors: [],
    };
  }

  async getTopRemovedProducts(): Promise<{ productId: string; productName: string; removeQuantity: number }[]> {
    const response = await this.axiosInstance.get('/actions/problematicProducts');
    return response.data;
  }

  async getTopUsersByRemove(): Promise<{ createdBy: string; totalRemovedQuantity: number }[]> {
    const response = await this.axiosInstance.get('/actions/topUsersByRemove');
    return response.data;
  }
}
