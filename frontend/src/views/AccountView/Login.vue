<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';

const store = useUserDataStore();
const router = useRouter();

const email = ref('admin@taltech.ee');
const password = ref('Foo.Bar.1');
const error = ref<string | null>(null);

const doLogin = async () => {
  error.value = null;
  const identityService = new IdentityService();
  const response = await identityService.login(email.value, password.value);

  if (response?.data) {
    store.jwt = response.data.jwt;
    store.refreshToken = response.data.refreshToken;
    store.firstName = response.data.firstName;
    store.lastName = response.data.lastName;
    store.email = response.data.email;
    store.username = response.data.username;
    store.role = response.data.roles?.[0] ?? null;

    router.push({ name: 'Home' });
  } else {
    const rawError = response.errors?.[0];

    if (rawError?.includes('User/Password problem') || rawError?.includes('404')) {
      error.value = 'Invalid email or password';
    } else if (rawError?.includes('Network Error')) {
      error.value = 'Server unreachable';
    } else {
      error.value = 'Login failed. Please try again.';
    }
  }
};
</script>

<template>
  <main class="flex items-center justify-center min-h-[calc(100vh-150px)] p-8">
    <div class="bg-black text-white p-8 rounded-2xl shadow-xl w-full max-w-md">
      <img src="@/assets/apollo-logo.png" alt="Apollo logo" class="mx-auto mb-6 w-40" />

      <form @submit.prevent="doLogin" class="space-y-5">
        <div>
          <label for="email" class="block mb-1 font-semibold">{{ $t('Email') }}</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            class="w-full px-4 py-2 rounded-lg bg-gray-100 text-black focus:outline-none focus:ring-2 focus:ring-yellow-400"
          />
        </div>

        <div>
          <label for="password" class="block mb-1 font-semibold">{{ $t('Password') }}</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
            class="w-full px-4 py-2 rounded-lg bg-gray-100 text-black focus:outline-none focus:ring-2 focus:ring-yellow-400"
          />
        </div>

        <button
          type="submit"
          class="w-full py-3 rounded-lg font-bold text-black bg-gradient-to-r from-yellow-500 to-yellow-300 hover:from-yellow-600 hover:to-yellow-400 transition"
        >
          {{ $t('Login') }}
        </button>

        <p v-if="error" class="text-red-500 text-sm mt-2 text-center">{{ error }}</p>
      </form>
    </div>
  </main>
</template>
