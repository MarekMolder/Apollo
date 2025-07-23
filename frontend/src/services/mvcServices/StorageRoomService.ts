import {BaseEntityService} from "@/services/BaseEntityService.ts";
import type {IStockAudit} from "@/domain/logic/IStockAudit.ts";
import type {IStorageRoom} from "@/domain/logic/IStorageRoom.ts";
import type {IResultObject} from "@/types/IResultObject.ts";

export class StorageRoomService extends BaseEntityService<IStorageRoom> {
  constructor() {
    super('storageRooms');
  }

  async getByInventoryId(id: string): Promise<IResultObject<IStorageRoom[]>> {
    const response = await this.axiosInstance.get(`/storagerooms/inventory/${id}`);
    return { data: response.data };
  }

  async getByInventoryId2(invId: string) {
    return await this.axiosInstance
      .get<IStorageRoom[]>(`storagerooms/byinventory/${invId}`)
      .then(r => ({ data: r.data, errors: [] }))
      .catch(e => ({ errors: [e.message] }));
  }
}
