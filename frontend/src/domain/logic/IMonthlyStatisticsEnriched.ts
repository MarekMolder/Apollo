import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IMonthlyStatisticsEnriched extends IDomainId {
  productId: string;
  productName: string;
  productCode: string;
  productUnit: string;
  storageRoomId: string;
  storageRoomName: string;
  totalRemovedQuantity: number;
  year: number;
  month: number;
}
