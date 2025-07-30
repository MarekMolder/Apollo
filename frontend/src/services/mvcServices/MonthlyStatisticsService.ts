import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {ICurrentStock} from "@/domain/logic/ICurrentStock.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {ICurrentStockEnriched} from "@/domain/logic/ICurrentStockEnriched.ts";
import type {IMonthlyStatistics} from "@/domain/logic/IMonthlyStatistics.ts";
import type {IActionEnriched} from "@/domain/logic/IActionEnriched.ts";
import type {IMonthlyStatisticsEnriched} from "@/domain/logic/IMonthlyStatisticsEnriched.ts";

export class MonthlyStatisticsService extends BaseEntityService<IMonthlyStatistics> {
  constructor() {
    super('monthlyStatistics');
  }

  async getByStorageRoomId(id: string): Promise<IResultObject<IMonthlyStatisticsEnriched[]>> {
    const response = await this.axiosInstance.get(`/monthlyStatistics/bystorageroom/${id}`);
    return { data: response.data };
  }

  async getEnrichedMonthlyStatistics(): Promise<IResultObject<IMonthlyStatisticsEnriched[]>> {
    const response = await this.axiosInstance.get('/monthlyStatistics/enrichedMonthlyStatistics');
    return {
      data: response.data as IMonthlyStatisticsEnriched[],
      errors: [],
    };
  }
}
