<script setup lang="ts">
import { useUserDataStore } from "@/stores/userDataStore.ts";
import { IdentityService } from "@/services/IdentityService.ts";
import { ref } from "vue";
import { RouterLink } from "vue-router";
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()

// Services
const identityService = new IdentityService();

// Store
const store = useUserDataStore();

// ??
const currentPassword = ref('');
const newPassword = ref('');
const confirmNewPassword = ref('');

// Messages errors/success
const message = ref('');

// Password edit function
const changePassword = async () => {
  if (newPassword.value !== confirmNewPassword.value) {
    message.value = 'New password and confirmation password arent same';
    return;
  }

  const result = await identityService.changePassword({
    email: store.email!,
    currentPassword: currentPassword.value,
    newPassword: newPassword.value,
  });

  message.value = result.errors
    ? `Error: ${result.errors.join(', ')}`
    : 'Password changed successfully!';
};
</script>

<template>
  <main
    :class="[
    sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]',
    'transition-all duration-300 px-6 sm:px-8 py-8',
    'flex items-start justify-center text-white font-[Inter,sans-serif] bg-transparent',
    'pt-16 sm:pt-20 min-h-screen'
  ]"
  >
    <!-- Keskosa wrapper, piiratud laius -->
    <div class="w-full max-w-lg">
      <!-- Header väljaspool kasti, keskel -->
      <section class="mb-6 text-center">
        <h1
          class="text-3xl sm:text-4xl font-[Playfair_Display] font-bold tracking-[0.02em]
                 drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]"
        >
          <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
            {{ $t('Change password') }}
          </span>
        </h1>
        <div class="mt-4 mx-auto h-px w-64 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
        <p class="mt-3 text-sm text-neutral-400">
          {{ $t('Update your password securely') }}
        </p>
      </section>

      <!-- Kaart -->
      <section>
        <div
          class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-5 sm:p-6
                 shadow-[0_0_0_1px_rgba(255,255,255,0.02),_0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl"
        >
          <form @submit.prevent="changePassword" class="grid grid-cols-1 gap-4">
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Enter current password') }}
              </label>
              <input
                v-model="currentPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Enter current password') as string"
              />
            </div>

            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Enter new password') }}
              </label>
              <input
                v-model="newPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Enter new password') as string"
              />
            </div>

            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Confirm new password') }}
              </label>
              <input
                v-model="confirmNewPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Confirm new password') as string"
              />
            </div>

            <div class="mt-2 flex flex-col sm:flex-row gap-3 sm:justify-between">
              <button
                type="submit"
                class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                       border-1 border-neutral-700 bg-gradient-to-br from-[#ffaa33]/20 via-[#ffaa33]/10 to-transparent text-[#ffaa33]
                       shadow-[0_0_0_1px_rgba(255,170,51,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                       hover:from-[#ffaa33]/30 hover:via-[#ffaa33]/20 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-[#ffaa33]/30 transition w-full sm:w-auto"
              >
                {{ $t('Change password') }}
              </button>

              <RouterLink
                to="/account"
                class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                       border-1 border-neutral-700 bg-white/5 text-neutral-200 no-underline
                       hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition w-full sm:w-auto text-center"
              >
                {{ $t('Back') }}
              </RouterLink>
            </div>

            <p
              v-if="message"
              :class="message.includes('Error')
                ? 'mt-2 text-center font-medium px-4 py-2 rounded-md bg-red-500/10 border border-red-500/20 text-red-400'
                : 'mt-2 text-center font-medium px-4 py-2 rounded-md bg-emerald-500/10 border border-emerald-500/20 text-emerald-400'"
            >
              {{ message }}
            </p>
          </form>
        </div>
      </section>
    </div>
  </main>
</template>

