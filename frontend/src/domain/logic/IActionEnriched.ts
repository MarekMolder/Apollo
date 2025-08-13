import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IActionEnriched extends IDomainId {
  quantity: number;
  status: string;
  actionTypeId: string;
  actionTypeName: string;
  reasonId: string | null;
  reasonDescription: string | null;
  productId: string;
  productName: string;
  productUnit: string;
  storageRoomId: string;
  storageRoomName: string;
  createdBy: string
  createdAt: string
}
