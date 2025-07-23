import type {IDomainId} from "@/domain/IDomainId.ts";

export interface ICurrentStock extends IDomainId {
  quantity: number;
  updatedAt: string;
  productId: string;
  StorageRoomId: string;
}
