<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { IdentityService } from '@/services/IdentityService';
import { useUserDataStore } from '@/stores/userDataStore';

// Services
const identityService = new IdentityService();

// Store and Router
const router = useRouter();
const store = useUserDataStore();

// ??
const email = ref('');
const password = ref('');
const firstName = ref('');
const lastName = ref('');

// Messages errors/success
const validationError = ref<string | null>(null);
const successMessage = ref<string | null>(null);

const isAdmin = store.roles === 'admin';

// Register function
const doRegister = async () => {
  validationError.value = null;
  successMessage.value = null;

  const result = await identityService.register({
    email: email.value,
    password: password.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });

  if (result?.data) {
    if (!isAdmin) {
      store.jwt = result.data.jwt;
      store.refreshToken = result.data.refreshToken;
      router.push({ name: 'Home' });
    } else {
      successMessage.value = 'Account successfully created';
      email.value = '';
      password.value = '';
      firstName.value = '';
      lastName.value = '';
    }
  } else {
    validationError.value = result.errors?.[0] || 'Registration failed';
  }
};
</script>

<template>
  <main
    class="flex justify-center items-center h-full px-4 sm:px-8 py-24 font-['Segoe_UI'] text-white"
  >
    <div
      class="w-full max-w-[500px] bg-[rgba(20,20,20,0.85)] backdrop-blur-md rounded-[16px] p-6 sm:p-8 md:p-10 shadow-[0_0_16px_rgba(255,165,0,0.2)] flex flex-col gap-4"
    >
      <RouterLink
        to="/"
        class="text-[#ffaa33] text-3xl sm:text-4xl font-bold text-center mb-4 drop-shadow-[0_0_6px_rgba(255,170,51,0.6)] no-underline"
      >
        Apollo
      </RouterLink>

      <div
        v-if="validationError"
        class="text-center bg-[rgba(255,80,80,0.2)] text-[#ff6b6b] border border-[rgba(255,80,80,0.4)] font-bold text-sm p-3 rounded"
      >
        {{ validationError }}
      </div>

      <div
        v-if="successMessage"
        class="text-center bg-[rgba(80,255,160,0.2)] text-[#80ffaa] border border-[rgba(80,255,160,0.4)] font-bold text-sm p-3 rounded"
      >
        {{ successMessage }}
      </div>

      <form @submit.prevent="doRegister" class="flex flex-col gap-4">
        <div>
          <label for="firstName" class="font-semibold mb-1 block text-[#f0f0f0]">
            {{ $t('Firstname') }}
          </label>
          <input
            v-model="firstName"
            type="text"
            id="firstName"
            required
            class="w-full px-3 py-2 rounded-lg bg-[#2a2a2a] text-white text-base border-none outline-none transition focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div>
          <label for="lastName" class="font-semibold mb-1 block text-[#f0f0f0]">
            {{ $t('Lastname') }}
          </label>
          <input
            v-model="lastName"
            type="text"
            id="lastName"
            required
            class="w-full px-3 py-2 rounded-lg bg-[#2a2a2a] text-white text-base border-none outline-none transition focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div>
          <label for="email" class="font-semibold mb-1 block text-[#f0f0f0]">
            {{ $t('Email') }}
          </label>
          <input
            v-model="email"
            type="email"
            id="email"
            required
            class="w-full px-3 py-2 rounded-lg bg-[#2a2a2a] text-white text-base border-none outline-none transition focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div>
          <label for="password" class="font-semibold mb-1 block text-[#f0f0f0]">
            {{ $t('Password') }}
          </label>
          <input
            v-model="password"
            type="password"
            id="password"
            required
            class="w-full px-3 py-2 rounded-lg bg-[#2a2a2a] text-white text-base border-none outline-none transition focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <button
          type="submit"
          class="mt-4 bg-gradient-to-r from-[#ff8c00] to-[#ffa500] text-white font-bold py-3 rounded-lg text-base transition hover:from-[#ffa500] hover:to-[#ffcc00] hover:scale-105"
        >
          {{ $t('Register') }}
        </button>
      </form>
    </div>
  </main>
</template>

