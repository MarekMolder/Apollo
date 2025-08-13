<script setup lang="ts">
import { useUserDataStore } from "@/stores/userDataStore.ts";
import { IdentityService } from "@/services/IdentityService.ts";
import { ref } from "vue";
import { RouterLink } from "vue-router";

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
  <main class="min-h-[80vh] flex justify-center items-center px-4 py-8 sm:px-8 font-['Inter'] text-white">
    <section class="w-full max-w-[480px] bg-[rgba(20,20,20,0.85)] rounded-[16px] p-6 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md">
      <h1 class="text-xl sm:text-2xl text-center text-[#ffaa33] mb-8 sm:mb-12 drop-shadow-[0_0_8px_rgba(255,170,51,0.2)]">
        🔐 {{ $t('Change password') }}
      </h1>

      <form @submit.prevent="changePassword" class="flex flex-col gap-4">
        <div>
          <input
            v-model="currentPassword"
            type="password"
            :placeholder="$t('Enter current password')"
            class="w-full bg-[rgba(60,60,60,0.7)] text-white rounded-lg px-4 py-2 text-base focus:outline-none focus:bg-[rgba(80,80,80,0.85)] focus:border focus:border-[#ffaa33] transition"
          />
        </div>

        <div>
          <input
            v-model="newPassword"
            type="password"
            :placeholder="$t('Enter new password')"
            class="w-full bg-[rgba(60,60,60,0.7)] text-white rounded-lg px-4 py-2 text-base focus:outline-none focus:bg-[rgba(80,80,80,0.85)] focus:border focus:border-[#ffaa33] transition"
          />
        </div>

        <div>
          <input
            v-model="confirmNewPassword"
            type="password"
            :placeholder="$t('Confirm new password')"
            class="w-full bg-[rgba(60,60,60,0.7)] text-white rounded-lg px-4 py-2 text-base focus:outline-none focus:bg-[rgba(80,80,80,0.85)] focus:border focus:border-[#ffaa33] transition"
          />
        </div>

        <div class="flex flex-col sm:flex-row justify-between items-center gap-4 mt-4">
          <button
            type="submit"
            class="w-full sm:w-auto bg-gradient-to-r from-[#ff8c00] to-[#ffa500] text-white font-bold text-sm rounded-lg px-6 py-2 shadow-md hover:from-[#ffc56e] hover:to-[#ffa726] transition"
          >
            {{ $t('Change password') }}
          </button>

          <RouterLink
            to="/account"
            class="w-full sm:w-auto text-center text-[#ffaa33] text-sm underline hover:text-[#ffd28f] transition"
          >
            {{ $t('Back') }}
          </RouterLink>
        </div>

        <p
          v-if="message"
          :class="message.includes('Viga')
            ? 'mt-4 text-center font-semibold text-sm bg-[rgba(255,80,80,0.15)] border border-[rgba(255,80,80,0.6)] text-[#ff5f5f] px-4 py-2 rounded-lg'
            : 'mt-4 text-center font-semibold text-sm bg-[rgba(0,255,100,0.1)] border border-[rgba(0,255,100,0.4)] text-[#9effb1] px-4 py-2 rounded-lg'"
        >
          {{ message }}
        </p>
      </form>
    </section>
  </main>
</template>
