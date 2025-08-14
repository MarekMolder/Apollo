<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { UserWithRolesDto } from "@/types/UserWithRolesDto";
import type { AppRole } from "@/domain/logic/AppRole";
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()

// Serivces
const roleService = new RoleService();

// Entity's
const users = ref<UserWithRolesDto[]>([]);
const allRoles = ref<AppRole[]>([]);

// Error message
const validationError = ref("");

// Get users and roles
onMounted(async () => {
  await fetchAllRoles();
  await fetchUsers();
});

const fetchUsers = async () => {
  try {
    users.value = await roleService.getAllUsersWithRoles();
  } catch (e: any) {
    validationError.value = e.message || "Error loading users";
  }
};

const fetchAllRoles = async () => {
  try {
    allRoles.value = await roleService.getAllRoles();
  } catch (e: any) {
    validationError.value = e.message || "Error loading roles";
  }
};

// Role remove function
const removeRole = async (userId: string, roleName: string) => {
  const role = allRoles.value.find(r => r.name === roleName);
  if (!role) {
    validationError.value = `Role - '${roleName}' does not exist`;
    return;
  }

  const result = await roleService.removeRoleFromUser(userId, role.id);
  if (!result.errors!.length) {
    await fetchUsers();
  } else {
    validationError.value = result.errors!.join(", ");
  }
};

</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-6 sm:p-8 text-white font-[Inter,sans-serif] bg-transparent max-w-screen-2xl',
      sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]'
    ]"
  >
    <!-- Header väljaspool kaarti -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]"
      >
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          {{ $t('Users and roles') }}
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-64 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">
        {{ $t('View users and manage their roles') }}
      </p>
    </section>

    <!-- Kaart -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-5 sm:p-6 shadow-[0_0_0_1px_rgba(255,255,255,0.02),_0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl"
      >
        <!-- Veateade -->
        <p
          v-if="validationError"
          class="mb-6 text-center text-sm font-medium px-4 py-2 rounded-md bg-red-500/10 border border-red-500/20 text-red-400"
        >
          {{ validationError }}
        </p>

        <!-- Tabel -->
        <div class="overflow-x-auto rounded-[12px] border border-neutral-700">
          <table class="w-full min-w-[600px] text-left text-sm sm:text-base table-fixed">
            <colgroup>
              <col class="w-[22rem]" />  <!-- Email -->
              <col class="w-[18rem]" />  <!-- Full name -->
              <col />                    <!-- Roles -->
            </colgroup>

            <thead class="bg-neutral-900/70 text-neutral-300">
            <tr>
              <th class="px-4 py-3 font-medium">{{ $t('Email') }}</th>
              <th class="px-4 py-3 font-medium">{{ $t('Full name') }}</th>
              <th class="px-4 py-3 font-medium">{{ $t('Roles') }}</th>
            </tr>
            </thead>

            <tbody>
            <tr
              v-for="user in users"
              :key="user.id"
              class="border-t border-white/10 even:bg-white/5 hover:bg-white/10 transition"
            >
              <td class="px-4 py-3 align-top">{{ user.email }}</td>
              <td class="px-4 py-3 align-top">
                {{ user.firstName }} {{ user.lastName }}
              </td>
              <td class="px-4 py-3 align-top">
                  <span
                    v-for="role in user.roles"
                    :key="role"
                    class="inline-flex items-center gap-2 px-3 py-1 rounded-full text-xs sm:text-sm font-medium
                           ring-1 ring-[#ffaa33]/35 bg-white/[0.06] text-[#ffaa33] mr-2 mb-2"
                  >
                    {{ role }}
                    <button
                      @click="removeRole(user.id, role)"
                      class="inline-flex items-center justify-center w-5 h-5 rounded-full
                             ring-1 ring-rose-400/30 bg-rose-500/10 text-rose-300
                             hover:bg-rose-500/15 hover:text-white transition"
                      :title="$t('Remove role') as string"
                    >
                      ×
                    </button>
                  </span>
              </td>
            </tr>

            <tr v-if="users.length === 0">
              <td colspan="3" class="px-4 py-10 text-center text-neutral-400">
                {{ $t('No data to display') }}
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  </main>
</template>
