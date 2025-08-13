import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IRecipeComponentEnriched extends IDomainId {
  productRecipeId: string;
  productRecipeName: string;
  productRecipeCode: string;
  productRecipeUnit: string;
  productRecipeVolume: number;
  productRecipeVolumeUnit: string;
  componentProductId: string;
  componentProductName: string;
  componentProductCode: string;
  componentProductUnit: string;
  componentProductVolume: number;
  componentProductVolumeUnit: string;
  amount: number;
}
