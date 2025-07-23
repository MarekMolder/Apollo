import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IReason} from "@/domain/logic/IReason.ts";

export class ReasonService extends BaseEntityService<IReason> {
  constructor() {
    super('reasons');
  }
}
