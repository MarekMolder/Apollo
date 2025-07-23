import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IAction extends IDomainId {
  quantity: number;
  status: string;
  actionTypeId: string;
  reasonId: string | null;
  supplierId: string | null;
  productId: string | null;
  stockAuditId: string | null;
  storageRoomId: string | null;
}
