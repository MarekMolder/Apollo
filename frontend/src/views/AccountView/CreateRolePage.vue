<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RoleService } from '@/services/RoleService'
import type { AppRole } from '@/domain/logic/AppRole'

// Services
const roleService = new RoleService()

// Entitys
const roles = ref<AppRole[]>([])

// ??
const newRoleName = ref('')

// Messages errors/success
const validationError = ref('')
const successMessage = ref('')

// Get roles
const fetchRoles = async () => {
  try {
    roles.value = await roleService.getAllRoles()
  } catch (e: any) {
    validationError.value = e.message
  }
}

onMounted(fetchRoles)

// Role create function
const createRole = async () => {
  validationError.value = ''
  successMessage.value = ''

  if (!newRoleName.value.trim()) {
    validationError.value = 'Role name cant be empty'
    return
  }

  const result = await roleService.createRole(newRoleName.value.trim())
  if (result.errors!.length) {
    validationError.value = result.errors!.join(', ')
  } else {
    successMessage.value = result.data!
    newRoleName.value = ''
    await fetchRoles()
  }
}
</script>

<template>
  <main class="px-4 sm:px-6 lg:ml-64 p-6 py-10 max-w-screen-xl mx-auto text-white font-['Inter']">
    <!-- Header -->
    <section class="mb-8 text-center sm:text-left">
      <h1 class="text-3xl sm:text-4xl font-extrabold text-[#ffaa33] drop-shadow-[0_0_10px_rgba(255,170,51,0.25)] mb-1">
        👥 Roles
      </h1>
      <p class="text-sm text-[#bbbbbb] opacity-85">Manage access roles for your system</p>
    </section>

    <!-- Form -->
    <div class="bg-[rgba(30,30,30,0.6)] backdrop-blur-md rounded-2xl p-6 shadow-[0_0_12px_rgba(255,170,51,0.05)] mb-8">
      <form @submit.prevent="createRole" class="flex flex-col sm:flex-row gap-4">
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
        v-if="validationError"
        class="mt-4 text-sm font-medium px-4 py-2 rounded-md bg-[rgba(255,80,80,0.15)] border-1 border-[#ffaa33] text-[#ff5f5f]"
      >
        {{ validationError }}
      </p>
      <p
        v-if="successMessage"
        class="mt-4 text-sm font-medium px-4 py-2 rounded-md bg-[rgba(0,255,100,0.1)] border-1 border-[#ffaa33] text-[#9effb1]"
      >
        {{ successMessage }}
      </p>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto rounded-2xl">
      <table class="w-full min-w-[400px] border-collapse text-left text-white bg-[rgba(20,20,20,0.5)] backdrop-blur-md shadow-inner shadow-[inset_0_0_20px_rgba(255,165,0,0.05)]">
        <thead class="bg-[#ffaa33] text-black">
          <tr>
            <th class="py-3 px-4 text-sm sm:text-base">ID</th>
            <th class="py-3 px-4 text-sm sm:text-base">Role Name</th>
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

