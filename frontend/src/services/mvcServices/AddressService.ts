import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {IProductEnriched} from "@/domain/logic/IProductEnriched.ts";
import type {IAddressEnriched} from "@/domain/logic/IAddressEnriched.ts";

export class AddressService extends BaseEntityService<IAddress> {
  constructor() {
    super('addresses');
  }

  async getEnrichedAddresses(): Promise<IResultObject<IAddressEnriched[]>> {
    const response = await this.axiosInstance.get('/addresses/enrichedaddresses');
    return {
      data: response.data as IAddressEnriched[],
      errors: [],
    };
  }
}
