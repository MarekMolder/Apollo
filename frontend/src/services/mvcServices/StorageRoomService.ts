import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IStorageRoom} from "@/domain/logic/IStorageRoom.ts";
import type {IResultObject} from "@/types/IResultObject.ts";
import type {IStorageRoomEnriched} from "@/domain/logic/IStorageRoomEnriched.ts";

export class StorageRoomService extends BaseEntityService<IStorageRoom> {
  constructor() {
    super('storageRooms');
  }

  async getEnrichedStorageRooms(): Promise<IResultObject<IStorageRoomEnriched[]>> {
    const response = await this.axiosInstance.get('/storageRooms/enrichedstoragerooms');
    return {
      data: response.data as IStorageRoomEnriched[],
      errors: [],
    };
  }
}
