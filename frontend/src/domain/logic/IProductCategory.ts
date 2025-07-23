import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IProductCategory extends IDomainId {
  name: string;
  endedAt: number;
}
