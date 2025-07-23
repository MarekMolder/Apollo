<script setup lang="ts">
import {useUserDataStore} from "@/stores/userDataStore.ts";
import {IdentityService} from "@/services/IdentityService.ts";
import {ref} from "vue";
import {RouterLink} from "vue-router";

const store = useUserDataStore();
const identityService = new IdentityService();

const message = ref('');
const currentPassword = ref('');
const newPassword = ref('');
const confirmNewPassword = ref('');

const changePassword = async () => {
  if (newPassword.value !== confirmNewPassword.value) {
    message.value = 'Uus parool ja kinnitus ei ühti!';
    return;
  }

  const result = await identityService.changePassword({
    email: store.email!,
    currentPassword: currentPassword.value,
    newPassword: newPassword.value
  });

  message.value = result.errors
    ? `Viga: ${result.errors.join(', ')}`
    : 'Parool muudetud edukalt!';
};

</script>

<template>
  <main class="password-wrapper">
    <section class="password-box">
      <h1 class="page-title">🔐 {{ $t('Change password') }}</h1>

      <form @submit.prevent="changePassword" class="password-form">
        <label>{{ $t('Current password') }}</label>
        <input
          v-model="currentPassword"
          type="password"
          :placeholder="$t('Enter current password')"
        />

        <label>{{ $t('New password') }}</label>
        <input
          v-model="newPassword"
          type="password"
          :placeholder="$t('Enter new password')"
        />

        <label>{{ $t('Confirm new password') }}</label>
        <input
          v-model="confirmNewPassword"
          type="password"
          :placeholder="$t('Confirm new password')"
        />

        <div class="buttons">
          <button type="submit" class="update-btn">{{ $t('Change password') }}</button>
          <RouterLink to="/account" class="cancel-btn">
            ← {{ $t('Back') }}
          </RouterLink>
        </div>

        <p v-if="message" :class="message.includes('Viga') ? 'error-msg' : 'success-msg'">
          {{ message }}
        </p>
      </form>
    </section>
  </main>
</template>

<style scoped>
.password-wrapper {
  min-height: 80vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 2rem;
  font-family: 'Inter', sans-serif;
  color: white;
}

.password-box {
  background: rgba(20, 20, 20, 0.85);
  backdrop-filter: blur(10px);
  border-radius: 16px;
  padding: 2rem 2.5rem;
  width: 100%;
  max-width: 480px;
  box-shadow: 0 0 20px rgba(255, 165, 0, 0.08);
  border: 1px solid rgba(255, 170, 51, 0.15);
}

.page-title {
  font-size: 2rem;
  font-weight: 800;
  text-align: center;
  color: #ffaa33;
  margin-bottom: 1.5rem;
  text-shadow: 0 0 8px rgba(255, 170, 51, 0.2);
}

.password-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.password-form label {
  font-weight: 600;
  font-size: 0.95rem;
}

.password-form input {
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border-radius: 10px;
  background: rgba(60, 60, 60, 0.7);
  border: none;
  color: white;
  transition: all 0.2s ease;
}

.password-form input:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}

.buttons {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 1rem;
}

.update-btn {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-weight: bold;
  font-size: 0.95rem;
  border-radius: 10px;
  padding: 0.6rem 1.4rem;
  border: none;
  cursor: pointer;
  box-shadow: 0 3px 8px rgba(0, 0, 0, 0.2);
}

.update-btn:hover {
  background: linear-gradient(to right, #ffc56e, #ffa726);
}

.cancel-btn {
  color: #ffaa33;
  font-weight: 600;
  font-size: 0.95rem;
  text-decoration: underline;
  transition: color 0.2s ease;
}

.cancel-btn:hover {
  color: #ffd28f;
}

.success-msg,
.error-msg {
  margin-top: 1rem;
  font-weight: 600;
  font-size: 0.95rem;
  text-align: center;
  padding: 0.6rem 1rem;
  border-radius: 10px;
}

.success-msg {
  background: rgba(0, 255, 100, 0.1);
  border: 1px solid rgba(0, 255, 100, 0.4);
  color: #9effb1;
}

.error-msg {
  background: rgba(255, 80, 80, 0.15);
  border: 1px solid rgba(255, 80, 80, 0.6);
  color: #ff5f5f;
}
</style>
