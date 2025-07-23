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
  <main class="login-container">
    <div class="login-card">
      <img src="@/assets/apollo-logo.png" class="logo" alt="Apollo logo" />
      <form @submit.prevent="doLogin">
        <div class="input-field">
          <label for="email">{{ $t('Email') }}</label>
          <input type="email" v-model="email" required />
        </div>
        <div class="input-field">
          <label for="password">{{ $t('Password') }}</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit" class="login-btn">{{ $t('Login') }}</button>
      </form>
    </div>
  </main>
</template>

<style scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: calc(100vh - 150px);
  padding: 2rem;
}

.login-card {
  background: black;
  padding: 2rem;
  border-radius: 20px;
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.4);
  width: 100%;
  max-width: 400px;
  box-sizing: border-box;
}

.logo {
  display: block;
  margin: 0 auto 1.5rem auto;
  width: 150px;
}

.input-field {
  margin-bottom: 1.25rem;
}

.input-field label {
  display: block;
  margin-bottom: 0.3rem;
  color: white;
  font-weight: 600;
}

.input-field input {
  width: 100%;
  padding: 0.6rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  background: #f4f4f4;
  box-sizing: border-box;
}

.login-btn {
  width: 100%;
  padding: 0.75rem;
  border-radius: 8px;
  border: none;
  background: linear-gradient(to right, #ff9900, #ffcc33);
  font-weight: bold;
  color: black;
  cursor: pointer;
  transition: background 0.3s ease;
}

.login-btn:hover {
  background: linear-gradient(to right, #ffa500, #f0e68c);
}

@media (max-width: 480px) {
  .card-bg {
    padding: 1.25rem;
    border-radius: 12px;
  }

  .logo {
    width: 80px;
    margin-bottom: 1rem;
  }

  .login-btn {
    font-size: 1rem;
    padding: 0.6rem;
  }
}
</style>
