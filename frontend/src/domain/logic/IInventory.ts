import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IInventory extends IDomainId {
  name: string;
  endedAt: string | null;
  addressId: string;
}
