import type {IDomainId} from "@/domain/IDomainId.ts";
import type {List} from "postcss/lib/list";

export interface IStorageRoomEnriched extends IDomainId {
  name: string;
  addressId: string;
  fullAddress: string;
  allowedRoles: string[];
}
