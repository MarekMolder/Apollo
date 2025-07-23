<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { IdentityService } from '@/services/IdentityService';

const route = useRoute();
const identityService = new IdentityService();

const user = ref<{
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  userName: string;
} | null>(null);

onMounted(async () => {
  try {
    const id = route.params.id as string;
    user.value = await identityService.getUserById(id);
  } catch (err) {
    console.error('Failed to load user:', err);
  }
});
</script>

<template>
  <div class="user-detail-card">
    <h2>User Detail</h2>
    <div v-if="user">
      <p><strong>Email:</strong> {{ user.email }}</p>
      <p><strong>First name:</strong> {{ user.firstName }}</p>
      <p><strong>Last name:</strong> {{ user.lastName }}</p>
      <p><strong>Username:</strong> {{ user.userName }}</p>
    </div>
    <p v-else class="loading">Loading...</p>
  </div>
</template>

<style scoped>
.user-detail-card {
  background-color: #1e1e1e;
  padding: 2rem;
  border-radius: 1rem;
  color: white;
  max-width: 500px;
  margin: 3rem auto;
  box-shadow: 0 6px 20px rgba(255, 165, 0, 0.2);
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.user-detail-card h2 {
  font-size: 1.6rem;
  font-weight: 600;
  color: orange;
  border-bottom: 2px solid orange;
  padding-bottom: 0.5rem;
  margin-bottom: 1rem;
  text-align: center;
}

.user-detail-card p {
  font-size: 1rem;
  line-height: 1.6;
  margin: 0;
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid #333;
  padding: 0.4rem 0;
}

.user-detail-card p strong {
  color: #ffcc66;
  width: 40%;
}

.user-detail-card p:last-of-type {
  border-bottom: none;
}

.user-detail-card .loading {
  font-style: italic;
  color: #aaa;
  text-align: center;
}
</style>
