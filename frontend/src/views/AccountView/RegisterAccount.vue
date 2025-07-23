<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { IdentityService } from '@/services/IdentityService';
import { useUserDataStore } from '@/stores/userDataStore';

const identityService = new IdentityService();
const router = useRouter();
const store = useUserDataStore();

const email = ref('');
const password = ref('');
const firstName = ref('');
const lastName = ref('');
const error = ref<string | null>(null);
const success = ref<string | null>(null);

const isAdmin = store.role === 'admin';

const doRegister = async () => {
  error.value = null;
  success.value = null;

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
      success.value = 'Account successfully created';
      email.value = '';
      password.value = '';
      firstName.value = '';
      lastName.value = '';
    }
  } else {
    error.value = result.errors?.[0] || 'Registration failed';
  }
};
</script>

<template>
  <main class="register-wrapper">
    <div class="register-box">
      <RouterLink class="custom-logo" to="/">Apollo</RouterLink>

      <div v-if="error" class="alert alert-warning">
        {{ error }}
      </div>

      <div v-if="success" class="alert alert-success">
        {{ success }}
      </div>

      <form @submit.prevent="doRegister">
        <label for="firstName">{{ $t('Firstname') }}</label>
        <input v-model="firstName" type="text" id="firstName" required />

        <label for="lastName">{{ $t('Lastname') }}</label>
        <input v-model="lastName" type="text" id="lastName" required />

        <label for="email">{{ $t('Email') }}</label>
        <input v-model="email" type="email" id="email" required />

        <label for="password">{{ $t('Password') }}</label>
        <input v-model="password" type="password" id="password" required />

        <button type="submit" class="register-button">{{ $t('Register') }}</button>
      </form>
    </div>
  </main>
</template>

<style scoped>
.register-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 6rem 2rem;
  min-height: 50vh;
  font-family: 'Segoe UI', sans-serif;
  color: white;
}

.register-box {
  background: rgba(20, 20, 20, 0.85);
  backdrop-filter: blur(8px);
  border-radius: 16px;
  padding: 2.5rem;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 8px 24px rgba(255, 170, 51, 0.2);
  border: 1px solid rgba(255, 170, 51, 0.5);
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.custom-logo {
  color: #ffaa33;
  font-size: 2.2rem;
  font-weight: bold;
  text-align: center;
  text-decoration: none;
  margin-bottom: 1rem;
  text-shadow: 0 0 6px rgba(255, 170, 51, 0.6);
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

label {
  font-weight: 600;
  margin-bottom: 0.2rem;
  color: #f0f0f0;
}

input {
  width: 100%;
  padding: 0.6rem 0.75rem;
  border-radius: 8px;
  border: none;
  background-color: #2a2a2a;
  color: white;
  font-size: 1rem;
  transition: background 0.3s, border 0.3s;
}

input:focus {
  outline: none;
  background: #1a1a1a;
  border: 1px solid orange;
}

.register-button {
  margin-top: 1rem;
  background: linear-gradient(to right, #ff8c00, #ffa500);
  border: none;
  padding: 0.75rem;
  border-radius: 8px;
  color: white;
  font-weight: bold;
  font-size: 1.05rem;
  cursor: pointer;
  transition: background 0.3s ease, transform 0.2s ease;
}

.register-button:hover {
  background: linear-gradient(to right, #ffa500, #ffcc00);
  transform: scale(1.02);
}

.alert,
.alert-success {
  text-align: center;
  padding: 0.75rem;
  font-weight: bold;
  border-radius: 8px;
  margin-bottom: 1rem;
  font-size: 0.95rem;
}

.alert {
  background-color: rgba(255, 80, 80, 0.2);
  color: #ff6b6b;
  border: 1px solid rgba(255, 80, 80, 0.4);
}

.alert-success {
  background-color: rgba(80, 255, 160, 0.2);
  color: #80ffaa;
  border: 1px solid rgba(80, 255, 160, 0.4);
}

@media (max-width: 600px) {
  .register-box {
    padding: 2rem;
  }

  .custom-logo {
    font-size: 2rem;
  }

  .register-button {
    font-size: 1rem;
  }
}
</style>
