<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { AppRole } from "@/domain/logic/AppRole";
import { useUserDataStore } from "@/stores/userDataStore";

const roleService = new RoleService();
const roles = ref<AppRole[]>([]);
const error = ref("");
const store = useUserDataStore();

const fetchRoles = async () => {
  console.log("JWT in store:", store.jwt);
  try {
    roles.value = await roleService.getAllRoles();
  } catch (e: any) {
    error.value = e.message;
  }
};

onMounted(fetchRoles);
</script>

<template>
  <main class="flex justify-center px-4 sm:px-6 lg:px-8 py-8 font-['Inter'] text-white">
    <section
      class="w-full max-w-4xl bg-[rgba(20,20,20,0.85)] rounded-2xl p-6 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md"
    >
      <h1
        class="text-center text-2xl sm:text-3xl md:text-4xl font-extrabold text-[#ffaa33] mb-6"
      >
        🎬 {{ $t('All roles') }}
      </h1>

      <p
        v-if="error"
        class="mb-6 text-center text-red-400 bg-red-900/30 rounded-md py-2 px-4"
      >
        {{ error }}
      </p>

      <div
        class="overflow-x-auto max-h-[400px] overflow-y-auto rounded-xl bg-[rgba(20,20,20,0.5)] shadow-inner shadow-[inset_0_0_10px_rgba(255,170,51,0.05)]"
      >
        <table class="w-full min-w-[400px] text-white text-base border-collapse">
          <thead class="bg-[#ffaa33] text-black">
            <tr>
              <th class="py-3 px-4 text-left">ID</th>
              <th class="py-3 px-4 text-left">{{ $t('Name') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="role in roles"
              :key="role.id"
              class="hover:bg-[rgba(255,170,51,0.08)]"
            >
              <td class="py-3 px-4 border-b border-white/10">{{ role.id }}</td>
              <td class="py-3 px-4 border-b border-white/10">{{ role.name }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  </main>
</template>
