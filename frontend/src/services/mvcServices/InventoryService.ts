import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {ISupplier} from "@/domain/logic/ISupplier.ts";
import type {IProductEnriched} from "@/domain/logic/IProductEnriched.ts";
import type {ISupplierEnriched} from "@/domain/logic/ISupplierEnriched.ts";
import type {IInventory} from "@/domain/logic/IInventory.ts";
import type {IInventoryEnriched} from "@/domain/logic/IInventoryEnriched.ts";

export class InventoryService extends BaseEntityService<IInventory> {
  constructor() {
    super('inventories');
  }

  async getEnrichedInventories(): Promise<IResultObject<IInventoryEnriched[]>> {
    const response = await this.axiosInstance.get('/inventories/enrichedinventories');
    return {
      data: response.data as IInventoryEnriched[],
      errors: [],
    };
  }
}
