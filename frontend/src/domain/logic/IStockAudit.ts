import type {IDomainId} from "@/domain/IDomainId.ts";

export interface IStockAudit extends IDomainId {
  storageroomId: string;
}
