import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IActionEnriched extends IDomainId {
  quantity: number;
  status: string;
  actionTypeId: string;
  actionTypeName: string;
  reasonId: string | null;
  reasonDescription: string | null;
  supplierId: string | null;
  supplierName: string | null;
  productId: string;
  productName: string;
  stockAuditId: string | null;
  storageRoomId: string;
  storageRoomName: string;
}
