import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IInventoryEnriched extends IDomainId {
  name: string;
  endedAt: string | null;
  addressId: string;
  fullAddress: string;
}
