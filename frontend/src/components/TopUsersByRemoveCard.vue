<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { ActionService } from '@/services/mvcServices/ActionService'

// ---------------- Services ----------------
const service = new ActionService()

// ---------------- Entities ----------------

interface UserRemoveStat {
  createdBy: string
  totalRemovals: number
}

const users = ref<UserRemoveStat[]>([])

// ---------------- Fetch ----------------
const fetchTopUsers = async () => {
  try {
    users.value = await service.getTopUsersByRemove()
  } catch (error) {
    console.error('Failed to fetch top users by remove:', error)
  }
}

onMounted(fetchTopUsers)
</script>

<template>
  <div
    class="h-full rounded-2xl border border-white/10 bg-neutral-900/70 p-5 sm:p-6 backdrop-blur-xl flex flex-col"
  >
    <!-- Header -->
    <div class="flex items-center justify-between">
      <h2 class="text-lg sm:text-xl font-semibold text-neutral-100 flex items-center gap-2">
        <span class="inline-flex h-6 w-6 items-center justify-center rounded-md bg-cyan-500/15 ring-1 ring-cyan-400/30 text-sm">👤</span>
        {{ $t('Most active removers') }}
      </h2>
      <span class="inline-flex h-8 items-center rounded-full border border-white/10 px-3 text-xs text-neutral-300">
        {{ users.length }}
      </span>
    </div>

    <div class="mt-3 h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent"></div>

    <!-- Content -->
    <div class="mt-4 flex-1">
      <ul
        v-if="users.length"
        class="space-y-1.5 max-h-56 overflow-y-auto pr-1"
        role="list"
        aria-live="polite"
      >
        <li
          v-for="(u, idx) in users"
          :key="u.createdBy"
          class="group grid grid-cols-[auto,1fr,auto] items-center gap-3 rounded-xl border border-white/5 bg-white/5 px-3 py-2.5
                 hover:bg-white/10 hover:border-white/10 transition-colors"
        >
          <!-- Rank -->
          <div
            class="shrink-0 h-7 w-7 grid place-items-center rounded-lg bg-white/5 text-xs font-semibold text-neutral-300 ring-1 ring-white/10"
            :aria-label="$t('Rank') + ' ' + (idx + 1)"
          >
            {{ idx + 1 }}
          </div>

          <!-- Name + Progress -->
          <div class="min-w-0">
            <p class="truncate text-neutral-100" :title="u.createdBy">
              {{ u.createdBy }}
            </p>

            <!-- Subtle progress bar -->
            <div
              class="mt-1 h-1.5 w-full rounded-full bg-white/5 ring-1 ring-white/10 overflow-hidden"
              role="progressbar"
              :aria-valuenow="u.totalRemovals"
              aria-valuemin="0"
              :aria-valuemax="(Math.max(...users.map(x => x.totalRemovals)) || 1)"
            >
              <div
                class="h-full rounded-full bg-cyan-400/60"
                :style="{
                  width:
                    (((u.totalRemovals / (Math.max(...users.map(x => x.totalRemovals)) || 1)) * 100).toFixed(0)) + '%'
                }"
              ></div>
            </div>
          </div>

          <!-- Count (no units) -->
          <span class="shrink-0 text-cyan-300 font-semibold text-sm">
            {{ u.totalRemovals }}
          </span>
        </li>
      </ul>

      <p v-else class="text-center italic text-neutral-400 py-8">
        {{ $t('No data found') }}
      </p>
    </div>
  </div>
</template>
