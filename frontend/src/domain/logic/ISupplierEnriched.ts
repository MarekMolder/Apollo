import type {IDomainId} from "@/domain/IDomainId.ts";

export interface ISupplierEnriched extends IDomainId {
  name: string;
  telephoneNr: string;
  email: string;
  addressId: string;
  fullAddress: string;
}
