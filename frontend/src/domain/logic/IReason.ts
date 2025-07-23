import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IReason extends IDomainId {
  description: string;
  endedAt: string;
}
