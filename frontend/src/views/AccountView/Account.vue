<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';
import { RouterLink } from 'vue-router';

const store = useUserDataStore();
const identityService = new IdentityService();

const firstName = ref('');
const lastName = ref('');
const userName = ref('');
const message = ref('');

onMounted(() => {
  firstName.value = store.firstName || '';
  lastName.value = store.lastName || '';
  userName.value = store.username || '';
});

const update = async () => {
  const result = await identityService.updateUserFields({
    email: store.email!,
    userName: userName.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });

  message.value = result.errors ? `Viga: ${result.errors.join(', ')}` : '✅ Konto uuendatud!';
};
</script>

<template>
  <main class="account-wrapper">
    <div class="account-card">
      <div class="card-header">
        <div class="avatar">👤</div>
        <h2>{{ $t('My account') }}</h2>
      </div>
      <div class="info-section">
        <p><strong>{{ $t('Firsname') }}:</strong> {{ store.firstName }}</p>
        <p><strong>{{ $t('Lastname') }}:</strong> {{ store.lastName }}</p>
        <p><strong>{{ $t('Username') }}:</strong> {{ store.username }}</p>
        <p><strong>{{ $t('Email') }}:</strong> {{ store.email }}</p>
      </div>
    </div>

    <div class="edit-card">
      <h2>{{ $t('Edit Profile') }}</h2>
      <form @submit.prevent="update">
        <label>{{ $t('Firsname') }}</label>
        <input v-model="firstName" type="text" placeholder="Enter your first name..." />

        <label>{{ $t('Lastname') }}</label>
        <input v-model="lastName" type="text" placeholder="Enter your last name..." />

        <label>{{ $t('Username') }}</label>
        <input v-model="userName" type="text" placeholder="Enter your username..." />

        <div class="actions">
          <button type="submit" class="btn-save">{{ $t('Save data') }}</button>
          <RouterLink to="/changepassword" class="link-password">
            {{ $t('Change password') }}
          </RouterLink>
        </div>

        <p class="status-message">{{ message }}</p>
      </form>
    </div>
  </main>
</template>

<style scoped>
.account-wrapper {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: flex-start;
  gap: 3rem;
  padding: 7rem;
  color: white;
  font-family: 'Segoe UI', sans-serif;
}

.account-card,
.edit-card {
  flex: 1 1 360px;
  max-width: 500px;
  background: rgba(20, 20, 20, 0.85);
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 8px 24px rgba(255, 170, 51, 0.2);
  border: 1px solid rgba(255, 170, 51, 0.5);
  backdrop-filter: blur(6px);
}

.card-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.avatar {
  width: 90px;
  height: 90px;
  background: #ffaa33;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2.8rem;
  box-shadow: 0 4px 12px rgba(255, 170, 51, 0.4);
}

.info-section {
  background: rgba(255, 255, 255, 0.05);
  padding: 1.5rem;
  border-radius: 12px;
  font-size: 0.95rem;
  margin-top: 2rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.info-section p {
  margin: 0;
  padding: 0.3rem 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.edit-card h2 {
  margin-bottom: 1.2rem;
  color: #ffaa33;
  text-align: center;
}

.edit-card form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

label {
  font-weight: 600;
  margin-bottom: 0.2rem;
}

input[type="text"] {
  padding: 0.6rem;
  border-radius: 8px;
  border: none;
  background-color: #2a2a2a;
  color: #fff;
  font-size: 1rem;
  transition: background 0.3s ease;
}

input:focus {
  outline: none;
  background: #1a1a1a;
  border: 1px solid orange;
}

.actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 1rem;
}

.btn-save {
  background: linear-gradient(to right, #ff8c00, #ffa500);
  border: none;
  padding: 0.6rem 1.4rem;
  border-radius: 8px;
  color: white;
  font-weight: bold;
  cursor: pointer;
  transition: background 0.3s ease;
}

.btn-save:hover {
  background: linear-gradient(to right, #ffa500, #ffcc00);
}

.link-password {
  color: #ffaa33;
  font-weight: bold;
  text-decoration: underline;
}

.status-message {
  margin-top: 1rem;
  text-align: center;
  font-weight: bold;
  color: lightgreen;
}
</style>
