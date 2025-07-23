<script setup lang="ts">
import { computed, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
import type { IResultObject } from "@/types/IResultObject";
import type { IActionEnriched } from "@/domain/logic/IActionEnriched";
import { ActionService } from "@/services/mvcServices/ActionService";

const service = new ActionService();
const data = reactive<IResultObject<IActionEnriched[]>>({
  data: [],
  errors: []
});

const router = useRouter();

const fetchPageData = async () => {
  try {
    const result = await service.getEnrichedActions();
    data.data = result.data;
    data.errors = result.errors;
  } catch (error) {
    console.error('Error fetching data:', error);
  }
};

onMounted(fetchPageData);

const filteredData = computed(() => {
  return data.data!.filter(item => item.status === 'Pending');
});

const goToPendingPage = () => {
  router.push('/actionrequest');
};
</script>

<template>
  <div class="pending-actions-card">
    <h2>{{ $t('Pending Actions') }}</h2>
    <ul v-if="filteredData.length">
      <li v-for="action in filteredData.slice(0, 10)" :key="action.id" class="action-item">
        <span class="type">{{ action.actionTypeName }}</span>
        <span class="product">{{ action.productName }}</span>
        <span class="quantity">{{ action.quantity }}</span>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('No pending actions') }}</p>
    <button @click="goToPendingPage" class="view-all-button">{{ $t('View All') }}</button>
  </div>
</template>

<style scoped>
.pending-actions-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(255, 165, 0, 0.2);
  color: #fff;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  transition: box-shadow 0.3s ease;
}

.pending-actions-card h2 {
  margin: 0;
  font-size: 1.4rem;
  font-weight: 600;
  color: orange;
  border-bottom: 2px solid orange;
  padding-bottom: 0.5rem;
}

ul {
  list-style: none;
  padding: 0;
  margin: 0;
  max-height: 220px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: orange #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}

ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}

ul::-webkit-scrollbar-thumb {
  background-color: orange;
  border-radius: 4px;
}

.action-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.4rem 0;
  border-bottom: 1px solid #333;
  font-size: 0.95rem;
}

.action-item:last-child {
  border-bottom: none;
}

.type {
  font-weight: 600;
  flex: 1;
}

.product {
  flex: 1;
  text-align: center;
}

.quantity {
  flex: 0.5;
  text-align: right;
  color: #ffb84d;
}

.empty {
  font-style: italic;
  color: #aaa;
  text-align: center;
}

.view-all-button {
  margin-top: auto;
  padding: 0.6rem 1rem;
  background-color: orange;
  border: none;
  border-radius: 8px;
  color: black;
  font-weight: bold;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.view-all-button:hover {
  background-color: #ffcc66;
}
</style>
