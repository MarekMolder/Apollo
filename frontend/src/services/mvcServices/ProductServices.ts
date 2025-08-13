import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IProduct} from "@/domain/logic/IProduct.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {IActionEnriched} from "@/domain/logic/IActionEnriched.ts";
import type {IProductEnriched} from "@/domain/logic/IProductEnriched.ts";

export class ProductService extends BaseEntityService<IProduct> {
  constructor() {
    super('products');
  }

  async getEnrichedProducts(): Promise<IResultObject<IProductEnriched[]>> {
    const response = await this.axiosInstance.get('/products/enrichedproducts');
    return {
      data: response.data as IProductEnriched[],
      errors: [],
    };
  }

  async getBySupplier(supplierId: string): Promise<IResultObject<IProductEnriched[]>> {
    const response = await this.axiosInstance.get(`/products/by-supplier/${supplierId}`);
    return {
      data: response.data as IProductEnriched[],
      errors: [],
    };
  }
}
