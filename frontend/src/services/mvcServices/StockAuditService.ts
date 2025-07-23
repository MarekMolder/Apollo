import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IStockAudit} from "@/domain/logic/IStockAudit.ts";

export class StockAuditService extends BaseEntityService<IStockAudit> {
  constructor() {
    super('stockaudits');
  }
}
