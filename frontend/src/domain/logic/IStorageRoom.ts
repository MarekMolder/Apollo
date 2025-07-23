import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IStorageRoom extends IDomainId {
  name: string;
  location: string;
  EndedAt: string;
}
