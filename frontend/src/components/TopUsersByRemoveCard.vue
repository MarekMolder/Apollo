<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { ActionService } from '@/services/mvcServices/ActionService'

const service = new ActionService()

interface UserRemoveStat {
  createdBy: string
  totalRemovedQuantity: number
}

const users = ref<UserRemoveStat[]>([])

const fetchTopUsers = async () => {
  try {
    users.value = await service.getTopUsersByRemove()
  } catch (error) {
    console.error('Failed to fetch top users by remove:', error)
  }
}

onMounted(fetchTopUsers)
</script>

<template>
  <div class="top-users-card">
    <h2>👤 {{ $t('Most Active Removers') }}</h2>
    <ul v-if="users.length">
      <li v-for="user in users" :key="user.createdBy" class="user-item">
        <span class="email">{{ user.createdBy }}</span>
        <span class="removed">{{ user.totalRemovedQuantity }} pcs</span>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('No data found') }}</p>
  </div>
</template>

<style scoped>
.top-users-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(0, 191, 255, 0.25);
  color: #fff;
  max-width: 350px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.top-users-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #00bfff;
  margin: 0;
  border-bottom: 2px solid #00bfff;
  padding-bottom: 0.5rem;
}

ul {
  list-style: none;
  padding: 0;
  margin: 0;
  max-height: 200px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: #00bfff #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}

ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}

ul::-webkit-scrollbar-thumb {
  background-color: #00bfff;
  border-radius: 4px;
}

.user-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.4rem 0;
  border-bottom: 1px solid #333;
  font-size: 0.95rem;
}

.user-item:last-child {
  border-bottom: none;
}

.email {
  font-weight: 600;
  flex: 1;
}

.removed {
  flex: 0.6;
  text-align: right;
  color: #88e0ff;
  font-weight: bold;
}

.empty {
  font-style: italic;
  color: #aaa;
  text-align: center;
}
</style>
