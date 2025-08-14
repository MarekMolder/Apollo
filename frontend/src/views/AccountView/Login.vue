<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';

// Services
const identityService = new IdentityService();

// Store and Router
const store = useUserDataStore();
const router = useRouter();

// ??
const email = ref('admin@taltech.ee');
const password = ref('Foo.Bar.1');

// Error message
const validationError = ref<string | null>(null);

// Login function
const doLogin = async () => {
  validationError.value = null;

  const response = await identityService.login(email.value, password.value);

  if (response?.data) {
    store.jwt = response.data.jwt;
    store.refreshToken = response.data.refreshToken;
    store.firstName = response.data.firstName;
    store.lastName = response.data.lastName;
    store.email = response.data.email;
    store.username = response.data.username;
    store.roles = response.data.roles ?? [];

    router.push({ name: 'Home' });
  } else {
    const rawError = response.errors?.[0];

    if (rawError?.includes('User/Password problem') || rawError?.includes('404')) {
      validationError.value = 'Invalid email or password';
    } else if (rawError?.includes('Network Error')) {
      validationError.value = 'Server unreachable';
    } else {
      validationError.value = 'Login failed. Please try again.';
    }
  }
};
</script>

<template>
  <main class="flex items-center justify-center h-full px-4 sm:px-8">

    <!-- Kaart -->
    <div class="bg-black text-white w-full max-w-sm sm:max-w-md md:max-w-lg p-6 sm:p-8 rounded-2xl shadow-xl">

      <!-- Logo -->
      <img src="@/assets/apollo-logo.png" alt="Apollo logo" class="mx-auto mb-6 w-32 sm:w-40" />

      <!-- Login vorm -->
      <form @submit.prevent="doLogin" class="space-y-5">

        <!-- Email -->
        <div>
          <label for="email" class="block mb-1 text-sm sm:text-base font-semibold">{{ $t('Email') }}</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            class="w-full px-4 py-2 rounded-lg bg-gray-100 text-black text-sm sm:text-base focus:outline-none focus:ring-2 focus:ring-yellow-400"
          />
        </div>

        <!-- Password -->
        <div>
          <label for="password" class="block mb-1 text-sm sm:text-base font-semibold">{{ $t('Password') }}</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
            class="w-full px-4 py-2 rounded-lg bg-gray-100 text-black text-sm sm:text-base focus:outline-none focus:ring-2 focus:ring-yellow-400"
          />
        </div>

        <!-- Submit -->
        <button
          type="submit"
          class="w-full h-11 inline-flex items-center justify-center rounded-xl text-sm font-semibold
                 border border-neutral-700 bg-gradient-to-br from-[#ffaa33]/20 via-[#ffaa33]/10 to-transparent text-[#ffaa33]
                 shadow-[0_0_0_1px_rgba(255,170,51,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                 hover:from-[#ffaa33]/30 hover:via-[#ffaa33]/20 hover:text-white
                 focus:outline-none focus:ring-2 focus:ring-[#ffaa33]/30 transition"
        >
          {{ $t('Login') }}
        </button>

        <!-- Error -->
        <p v-if="validationError" class="text-red-500 text-sm mt-2 text-center">{{
            validationError
          }}</p>
      </form>
    </div>
  </main>
</template>

