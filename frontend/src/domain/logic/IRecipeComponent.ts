import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IRecipeComponent extends IDomainId {
  productRecipeId: string;
  componentProductId: string;
  amount: number;
}
