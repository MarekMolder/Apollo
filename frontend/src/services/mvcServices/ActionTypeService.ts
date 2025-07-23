import axios from "axios";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IActionType} from "@/domain/logic/IActionType.ts";

export class ActionTypeService extends BaseEntityService<IActionType> {
  constructor() {
    super('actiontypes');
  }
}
