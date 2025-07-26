import type {IDomainId} from "@/domain/IDomainId.ts";
import type {List} from "postcss/lib/list";

export interface IStorageRoom extends IDomainId {
  name: string;
  addressId: string;
  allowedRoles: string[];
  EndedAt: string;
}
