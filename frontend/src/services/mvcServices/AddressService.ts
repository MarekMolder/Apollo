import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";

export class AddressService extends BaseEntityService<IAddress> {
  constructor() {
    super('addresses');
  }
}
