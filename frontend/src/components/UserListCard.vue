<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { IdentityService } from '@/services/IdentityService'
import { RouterLink } from 'vue-router'

const identityService = new IdentityService()

const users = ref<{ id: string; email: string; firstName: string; lastName: string; username: string }[]>([])

onMounted(async () => {
  try {
    users.value = await identityService.getAllUsers()
  } catch (err) {
    console.error('Failed to load users:', err)
  }
})
</script>

<template>
  <div class="user-list-card">
    <h2>👥  {{ $t('All Users') }}</h2>
    <ul v-if="users.length">
      <li v-for="user in users" :key="user.id" class="user-entry">
        <div class="info">
          <strong>{{ user.firstName }} {{ user.lastName }}</strong>
          <p>{{ user.email }}</p>
        </div>
        <RouterLink :to="`/users/${user.id}`" class="view-link">{{ $t('View') }}</RouterLink>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('No users found') }}</p>
  </div>
</template>

<style scoped>
.user-list-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(255, 255, 255, 0.05);
  color: white;
  max-width: 400px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.user-list-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #cccccc;
  margin: 0;
  border-bottom: 2px solid #333;
  padding-bottom: 0.5rem;
}

ul {
  list-style: none;
  margin: 0;
  padding: 0;
  max-height: 220px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: #666 #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}
ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}
ul::-webkit-scrollbar-thumb {
  background-color: #666;
  border-radius: 4px;
}

.user-entry {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.6rem 0;
  border-bottom: 1px solid #333;
  font-size: 0.95rem;
}

.user-entry:last-child {
  border-bottom: none;
}

.info {
  display: flex;
  flex-direction: column;
}

.info p {
  margin: 0;
  font-size: 0.85rem;
  color: #aaa;
}

.view-link {
  background-color: #333;
  color: #ddd;
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  font-weight: bold;
  text-decoration: none;
  transition: background-color 0.3s ease;
}

.view-link:hover {
  background-color: #444;
  color: white;
}

.empty {
  font-style: italic;
  color: #888;
  text-align: center;
}
</style>
