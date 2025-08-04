import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IMonthlyStatistics extends IDomainId {
  productId: string;
  storageRoomId: string;
  totalRemovedQuantity: number;
  year: number;
  month: number;
}
