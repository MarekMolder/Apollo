<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { UserWithRolesDto } from "@/types/UserWithRolesDto";
import type { AppRole } from "@/domain/logic/AppRole";

const roleService = new RoleService();
const users = ref<UserWithRolesDto[]>([]);
const allRoles = ref<AppRole[]>([]);
const error = ref("");

const fetchUsers = async () => {
  try {
    users.value = await roleService.getAllUsersWithRoles();
  } catch (e: any) {
    error.value = e.message || "Viga kasutajate laadimisel";
  }
};

const fetchAllRoles = async () => {
  try {
    allRoles.value = await roleService.getAllRoles();
  } catch (e: any) {
    error.value = e.message || "Viga rollide laadimisel";
  }
};

const removeRole = async (userId: string, roleName: string) => {
  const role = allRoles.value.find(r => r.name === roleName);
  if (!role) {
    error.value = `Rolli '${roleName}' ei leitud`;
    return;
  }

  const result = await roleService.removeRoleFromUser(userId, role.id);
  if (!result.errors!.length) {
    await fetchUsers();
  } else {
    error.value = result.errors!.join(", ");
  }
};

onMounted(async () => {
  await fetchAllRoles();
  await fetchUsers();
});
</script>

<template>
  <main class="flex justify-center px-4 py-8 font-['Inter'] text-white">
    <section class="w-full max-w-[1100px] bg-[rgba(20,20,20,0.85)] backdrop-blur-md rounded-2xl p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md">
      <h1 class="text-center text-3xl font-extrabold text-[#ffaa33] mb-6 drop-shadow-[0_0_10px_rgba(255,170,51,0.2)]">
        👤 {{ $t('Users and roles') }}
      </h1>

      <p v-if="error" class="text-red-400 bg-[rgba(255,80,80,0.15)] border border-[rgba(255,80,80,0.6)] text-center text-base font-medium px-4 py-2 mb-6 rounded-lg">
        {{ error }}
      </p>

      <div class="overflow-x-auto rounded-xl bg-[rgba(20,20,20,0.5)] shadow-inner shadow-[inset_0_0_10px_rgba(255,170,51,0.05)]">
        <table class="w-full min-w-[600px] border-collapse text-sm sm:text-base">
          <thead class="bg-[#ffaa33] text-black">
            <tr>
              <th class="px-2 py-2 sm:px-4 sm:py-3 text-left"> {{ $t('Email') }} </th>
              <th class="px-2 py-2 sm:px-4 sm:py-3 text-left"> {{ $t('Full name') }} </th>
              <th class="px-2 py-2 sm:px-4 sm:py-3 text-left"> {{ $t('Roles') }} </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="user in users"
              :key="user.id"
              class="hover:bg-[rgba(255,170,51,0.08)]"
            >
              <td class="px-2 py-2 sm:px-4 sm:py-3 border-b border-white/10 align-top">{{ user.email }}</td>
              <td class="px-2 py-2 sm:px-4 sm:py-3 border-b border-white/10 align-top">
                {{ user.firstName }} {{ user.lastName }}
              </td>
              <td class="px-2 py-2 sm:px-4 sm:py-3 border-b border-white/10 align-top">
                <span
                  v-for="role in user.roles"
                  :key="role"
                  class="inline-flex items-center text-[#ffaa33] bg-[rgba(255,165,0,0.1)] border-1 border-[#ffaa33] px-2 sm:px-3 py-0.5 sm:py-1 mr-1 mb-1 rounded-full text-xs sm:text-sm font-semibold"
                >
                  {{ role }}
                  <button
                    @click="removeRole(user.id, role)"
                    class="ml-2 text-[#ff5f5f] hover:bg-[rgba(255,80,80,0.15)] text-base font-bold rounded-full px-1"
                    title="Remove role"
                  >
                    ×
                  </button>
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  </main>
</template>
