<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { IdentityService } from '@/services/IdentityService'

// Services
const identityService = new IdentityService()

// Router
const route = useRoute()

// Empty user
const user = ref<{
  id: string
  email: string
  firstName: string
  lastName: string
  userName: string
} | null>(null)

// Get User details
onMounted(async () => {
  try {
    const id = route.params.id as string
    user.value = await identityService.getUserById(id)
  } catch (err) {
    console.error('Failed to load user:', err)
  }
})
</script>

<template>
  <main
    class="w-full h-full flex justify-center items-center px-4 py-12 sm:px-6 sm:py-16 md:px-12 md:py-20 text-white font-['Segoe_UI']"
  >
    <div
      class="w-full max-w-[500px] bg-[rgba(20,20,20,0.85)] rounded-[16px] p-5 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md"
    >
      <h2 class="text-[#ffaa33] text-center text-xl sm:text-2xl font-semibold mb-6">
        {{ $t('User Details') }}
      </h2>

      <div
        v-if="user"
        class="grid grid-cols-1 sm:grid-cols-[140px_1fr] gap-y-3 gap-x-4 text-left"
      >
        <div class="text-white font-medium">{{ $t('Email') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.email }}</div>

        <div class="text-white font-medium">{{ $t('Firstname') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.firstName }}</div>

        <div class="text-white font-medium">{{ $t('Lastname') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.lastName }}</div>

        <div class="text-white font-medium">{{ $t('Username') }}:</div>
        <div class="text-gray-200">{{ user.userName }}</div>
      </div>

      <p v-else class="italic text-gray-400 text-center mt-4">{{ $t('Loading...') }}</p>
    </div>
  </main>
</template>

