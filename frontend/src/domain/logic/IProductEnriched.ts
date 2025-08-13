import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IProductEnriched extends IDomainId {
  id: string;
  unit: string;
  volume: number;
  volumeUnit: string;
  code: string;
  name: string;
  price: number;
  quantity: number;
  productCategoryId: string;
  productCategoryName: string;
  supplierId: string;
  supplierName: string;
  supplierEmail: string;
  isComponent: boolean;
}
