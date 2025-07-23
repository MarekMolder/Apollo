import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IActionType extends IDomainId {
  name: string;
  endedAt: string;
  code: number;
}
