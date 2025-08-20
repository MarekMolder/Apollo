<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { IdentityService } from '@/services/IdentityService'
import { RouterLink } from 'vue-router'

// ---------------- Services ----------------
const identityService = new IdentityService()

// ---------------- Users ----------------
const users = ref<{ id: string; email: string; firstName: string; lastName: string; username: string }[]>([])

// ---------------- Fetch ----------------
onMounted(async () => {
  try {
    users.value = await identityService.getAllUsers()
  } catch (err) {
    console.error('Failed to load users:', err)
  }
})
</script>

<template>
  <div
    class="h-full rounded-2xl border border-white/10 bg-neutral-900/70 p-5 sm:p-6 backdrop-blur-xl flex flex-col"
  >
    <!-- Header -->
    <div class="flex items-center justify-between">
      <h2 class="text-lg sm:text-xl font-semibold text-neutral-100 flex items-center gap-2">
        <span
          class="inline-flex h-6 w-6 items-center justify-center rounded-md bg-white/10 ring-1 ring-white/20 text-sm"
        >👥</span>
        {{ $t('All users') }}
      </h2>
      <span
        class="inline-flex h-8 items-center rounded-full border border-white/10 px-3 text-xs text-neutral-300"
      >
        {{ users.length }}
      </span>
    </div>

    <div
      class="mt-3 h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent"
    ></div>

    <!-- Content -->
    <div class="mt-4 flex-1">
      <ul
        v-if="users.length"
        class="space-y-1.5 max-h-56 overflow-y-auto pr-1"
      >
        <li
          v-for="user in users"
          :key="user.id"
          class="group grid grid-cols-[1fr,auto] items-center gap-3 rounded-xl border border-white/5 bg-white/5 px-3 py-2.5
                 hover:bg-white/10 hover:border-white/10 transition-colors"
        >
          <!-- Name + Email -->
          <div class="min-w-0">
            <p
              class="truncate text-neutral-100 font-medium"
              :title="`${user.firstName} ${user.lastName}`"
            >
              {{ user.firstName }} {{ user.lastName }}
            </p>
            <p
              class="truncate text-neutral-400 text-xs"
              :title="user.email"
            >
              {{ user.email }}
            </p>
          </div>

          <RouterLink
            :to="`/users/${user.id}`"
            class="shrink-0 inline-flex items-center rounded-xl border border-neutral-700 px-3 py-1.5 text-xs font-medium text-neutral-200 hover:bg-white/10 transition-colors"
          >
            {{ $t('View') }}
          </RouterLink>
        </li>
      </ul>

      <div v-else class="text-center py-10">
        <div
          class="mx-auto mb-2 h-10 w-10 grid place-items-center rounded-xl bg-white/5 ring-1 ring-white/10"
        >📭</div>
        <p class="text-neutral-400 italic">
          {{ $t('No users found') }}
        </p>
      </div>
    </div>
  </div>
</template>

