import { BaseService } from "@/services/BaseService";
import type { IResultObject } from "@/types/IResultObject";
import type { AppRole } from "@/domain/logic/AppRole";
import type { AssignRoleDto } from "@/types/AssignRoleDto";
import type {UserWithRolesDto} from "@/types/UserWithRolesDto.ts";

export class RoleService extends BaseService {
  constructor() {
    super();
  }

  async getAllRoles(): Promise<AppRole[]> {
    const res = await this.axiosInstance.get("/roles/getallroles");
    return res.data;
  }

  async createRole(roleName: string): Promise<IResultObject<string>> {
    try {
      const res = await this.axiosInstance.post("/roles/createrole", roleName, {
        headers: { "Content-Type": "application/json" }
      });
      return { data: res.data.message, errors: [] };
    } catch (e: any) {
      return { errors: [e.response?.data || e.message] };
    }
  }

  async assignRoleToUser(dto: AssignRoleDto): Promise<IResultObject<string>> {
    try {
      const res = await this.axiosInstance.post("/roles/assignroletouser", dto);
      return { data: res.data.message, errors: [] };
    } catch (e: any) {
      return { errors: [e.response?.data || e.message] };
    }
  }

  async getAllUsersWithRoles(): Promise<UserWithRolesDto[]> {
    const res = await this.axiosInstance.get("/account/getalluserswithroles");
    return res.data;
  }

  async removeRoleFromUser(userId: string, roleId: string): Promise<IResultObject<string>> {
    try {
      const res = await this.axiosInstance.delete("/account/removerolefromuser", {
        params: { userId, roleId },
      });
      return { data: res.data.message, errors: [] };
    } catch (e: any) {
      return { errors: [e.response?.data || e.message] };
    }
  }

}
