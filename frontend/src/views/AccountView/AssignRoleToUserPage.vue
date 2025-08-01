<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RoleService } from '@/services/RoleService'
import { IdentityService } from '@/services/IdentityService'
import type { AppRole } from '@/domain/logic/AppRole'
import type { AssignRoleDto } from '@/types/AssignRoleDto'

const roleService = new RoleService()
const identityService = new IdentityService()

const users = ref<{ id: string; firstName: string; lastName: string }[]>([])
const roles = ref<AppRole[]>([])
const selectedUserId = ref('')
const selectedRoleId = ref('')
const message = ref('')
const error = ref('')

const assign = async () => {
  const dto: AssignRoleDto = {
    userId: selectedUserId.value,
    roleId: selectedRoleId.value,
  }
  const res = await roleService.assignRoleToUser(dto)
  if (res.errors!.length) {
    error.value = res.errors![0]
    message.value = ''
  } else {
    message.value = res.data!
    error.value = ''
  }
}

onMounted(async () => {
  users.value = await identityService.getAllUsers()
  roles.value = await roleService.getAllRoles()
})
</script>

<template>
  <main class="flex justify-center px-4 py-12 text-white font-['Inter']">
    <div
      class="w-full max-w-xl bg-[rgba(20,20,20,0.85)] backdrop-blur-md rounded-2xl p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)]"
    >
      <h1
        class="text-center text-3xl sm:text-4xl font-extrabold text-[#ffaa33] mb-8 drop-shadow-[0_0_10px_rgba(255,170,51,0.2)]"
      >
        🎭 {{ $t('Assign role to user') }}
      </h1>

      <select
        v-model="selectedUserId"
        class="w-full mb-4 px-4 py-2.5 text-base rounded-lg bg-[rgba(43,43,43,0.6)] border-1 border-[#ffaa33] text-white placeholder-white focus:outline-none focus:border-[#ffc266] transition"
      >
        <option disabled value="">Select user</option>
        <option v-for="u in users" :key="u.id" :value="u.id">
          {{ u.firstName }} {{ u.lastName }}
        </option>
      </select>

      <select
        v-model="selectedRoleId"
        class="w-full mb-4 px-4 py-2.5 text-base rounded-lg bg-[rgba(43,43,43,0.6)] border-1 border-[#ffaa33] text-white placeholder-white focus:outline-none focus:border-[#ffc266] transition"
      >
        <option disabled value="">{{ $t('Select role') }}</option>
        <option v-for="r in roles" :key="r.id" :value="r.id">{{ r.name }}</option>
      </select>

      <button
        class="w-full bg-gradient-to-r from-[#ffaa33] to-[#ff8c00] text-black font-bold text-base rounded-xl px-6 py-3 transition hover:from-[#ffc56e] hover:to-[#ffa726] shadow-[0_3px_10px_rgba(255,165,0,0.2)]"
        @click="assign"
      >
        {{ $t('Assign') }}
      </button>

      <p
        v-if="message"
        class="mt-4 text-center text-sm font-medium px-4 py-2 rounded-md bg-[rgba(0,255,100,0.1)] border-1 border-[#ffaa33] text-[#9effb1]"
      >
        {{ message }}
      </p>

      <p
        v-if="error"
        class="mt-4 text-center text-sm font-medium px-4 py-2 rounded-md bg-[rgba(255,80,80,0.15)] border-1 border-[#ffaa33] text-[#ff5f5f]"
      >
        {{ error }}
      </p>
    </div>
  </main>
</template>
