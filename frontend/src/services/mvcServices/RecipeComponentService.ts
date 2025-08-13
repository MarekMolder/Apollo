import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IRecipeComponent} from "@/domain/logic/IRecipeComponent.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {IRecipeComponentEnriched} from "@/domain/logic/IRecipeComponentEnriched.ts";

export class RecipeComponentService extends BaseEntityService<IRecipeComponent> {
  constructor() {
    super('recipeComponent');
  }

  async getEnrichedRecipeComponents(): Promise<IResultObject<IRecipeComponentEnriched[]>> {
    const response = await this.axiosInstance.get('/recipeComponent/enrichedRecipeComponents');
    return {
      data: response.data as IRecipeComponentEnriched[],
      errors: [],
    };
  }
}
