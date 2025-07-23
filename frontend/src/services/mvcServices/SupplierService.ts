import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {ISupplier} from "@/domain/logic/ISupplier.ts";
import type {IProductEnriched} from "@/domain/logic/IProductEnriched.ts";
import type {ISupplierEnriched} from "@/domain/logic/ISupplierEnriched.ts";

export class SupplierService extends BaseEntityService<ISupplier> {
  constructor() {
    super('suppliers');
  }

  async getEnrichedSuppliers(): Promise<IResultObject<ISupplierEnriched[]>> {
    const response = await this.axiosInstance.get('/suppliers/enrichedsuppliers');
    return {
      data: response.data as ISupplierEnriched[],
      errors: [],
    };
  }
}
