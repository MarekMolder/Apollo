import type {IDomainId} from "@/domain/IDomainId.ts";

export interface ICurrentStockEnriched extends IDomainId {
  quantity: number;
  updatedAt: string;
  productId: string;
  productName: string;
  productCode: string;
  StorageRoomId: string;
}
