import type {IDomainId} from "@/domain/IDomainId.ts";

export interface ISupplier extends IDomainId {
  name: string;
  telephoneNr: string;
  email: string;
  addressId: string;
}
