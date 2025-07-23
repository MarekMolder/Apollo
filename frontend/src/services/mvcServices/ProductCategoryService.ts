import axios from "axios";
import type {IProductCategory} from "@/domain/logic/IProductCategory.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import {BaseEntityService} from "@/services/BaseEntityService.ts";

export class ProductCategoryService extends BaseEntityService<IProductCategory> {
  constructor() {
    super('productCategories');
  }
}
