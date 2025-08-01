<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RoleService } from '@/services/RoleService'
import type { AppRole } from '@/domain/logic/AppRole'

const roleService = new RoleService()
const roles = ref<AppRole[]>([])
const error = ref('')
const success = ref('')
const newRoleName = ref('')

const fetchRoles = async () => {
  try {
    roles.value = await roleService.getAllRoles()
  } catch (e: any) {
    error.value = e.message
  }
}

const createRole = async () => {
  error.value = ''
  success.value = ''

  if (!newRoleName.value.trim()) {
    error.value = 'Rolli nimi ei tohi olla tühi'
    return
  }

  const result = await roleService.createRole(newRoleName.value.trim())
  if (result.errors!.length) {
    error.value = result.errors!.join(', ')
  } else {
    success.value = result.data!
    newRoleName.value = ''
    await fetchRoles()
  }
}

onMounted(fetchRoles)
</script>

<template>
  <main class="px-8 py-10 max-w-[1000px] mx-auto text-white font-['Inter']">
    <!-- Header -->
    <section class="mb-8">
      <h1
        class="text-4xl font-extrabold text-[#ffaa33] drop-shadow-[0_0_10px_rgba(255,170,51,0.25)] mb-1"
      >
        👥 Roles
      </h1>
      <p class="text-sm text-[#bbbbbb] opacity-85">Manage access roles for your system</p>
    </section>

    <!-- Form -->
    <div
      class="bg-[rgba(30,30,30,0.6)] backdrop-blur-md rounded-2xl p-6 shadow-[0_0_12px_rgba(255,170,51,0.05)] mb-8"
    >
      <form @submit.prevent="createRole" class="flex flex-wrap gap-4">
        <input
          v-model="newRoleName"
          type="text"
          placeholder="Enter new role name"
          class="flex-1 px-4 py-2.5 rounded-lg text-white placeholder-gray-300 bg-[rgba(43,43,43,0.6)] border-1 border-[#ffaa33] focus:outline-none focus:ring-1"
        />
        <button
          type="submit"
          class="bg-gradient-to-r from-[#ffaa33] to-[#ff8c00] text-black font-bold text-base rounded-lg px-6 py-2.5 hover:from-[#ffc56e] hover:to-[#ffa726] transition"
        >
          + Create
        </button>
      </form>
      <p
        v-if="error"
        class="mt-4 text-sm font-medium px-4 py-2 rounded-md bg-[rgba(255,80,80,0.15)] border border-[rgba(255,80,80,0.6)] text-[#ff5f5f]"
      >
        {{ error }}
      </p>
      <p
        v-if="success"
        class="mt-4 text-sm font-medium px-4 py-2 rounded-md bg-[rgba(0,255,100,0.1)] border border-[rgba(0,255,100,0.4)] text-[#9effb1]"
      >
        {{ success }}
      </p>
    </div>

    <!-- Table -->
    <div
      class="bg-[rgba(20,20,20,0.5)] rounded-2xl p-4 backdrop-blur-md shadow-inner shadow-[inset_0_0_20px_rgba(255,165,0,0.05)]"
    >
      <table class="w-full border-collapse text-left text-white">
        <thead class="bg-[#ffaa33] text-black">
          <tr>
            <th class="py-3 px-4">ID</th>
            <th class="py-3 px-4">Role Name</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="role in roles" :key="role.id" class="hover:bg-[rgba(255,170,51,0.1)]">
            <td class="py-3 px-4 border-b border-white/10">{{ role.id }}</td>
            <td class="py-3 px-4 border-b border-white/10">{{ role.name }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>
