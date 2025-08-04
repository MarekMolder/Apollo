import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IProduct extends IDomainId {
  unit: string;
  volume: number;
  code: string;
  name: string;
  price: number;
  quantity: number;
  productCategoryId: string;
  supplierId: string;
  isComponent: boolean;
}
