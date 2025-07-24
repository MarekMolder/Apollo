<script setup lang="ts">
import {computed, onMounted, reactive, ref} from "vue";
import { useRouter } from "vue-router";
import type { IResultObject } from "@/types/IResultObject";
import type {IActionEnriched } from "@/domain/logic/IActionEnriched.ts";
import {ActionService} from "@/services/mvcServices/ActionService.ts";

const requestIsOngoing = ref(false);
const data = reactive<IResultObject<IActionEnriched[]>>({ data: [], errors: [] });

const service = new ActionService();

const fetchPageData = async () => {
  try {
    const result = await service.getEnrichedActions();

    data.data = result.data;
    data.errors = result.errors;

  } catch (error) {
    console.error('Error fetching data:', error);
  } finally {
    requestIsOngoing.value = true;
  }
};

onMounted(async () => {
  await fetchPageData();
});

const updateStatus = async (id: string, newStatus: 'Accepted' | 'Declined') => {
  try {
    const response = await service.patchStatus(id, newStatus);
    console.log(response.message);
    await fetchPageData();
  } catch (error) {
    console.error('Status update failed', error);
  }
};

const selectedStatus = ref<'All' | 'Accepted' | 'Declined' | 'Pending'>('All');

const filteredData = computed(() => {
  if (selectedStatus.value === 'All') return data.data;
  return data.data!.filter(item => item.status === selectedStatus.value);
});
</script>

<template>
  <main class="request-page">
    <section class="card">
      <header class="card-header">
        <h1>{{ $t('Requests') }}</h1>
        <div class="card-actions">
          <div class="filter-group">
            <i class="bi bi-funnel"></i>
            <select v-model="selectedStatus">
              <option value="All">{{ $t('All') }}</option>
              <option value="Accepted">{{ $t('Accepted') }}</option>
              <option value="Declined">{{ $t('Declined') }}</option>
              <option value="Pending">{{ $t('Pending') }}</option>
            </select>
          </div>

          <router-link to="/createaction" class="btn-primary">
            <i class="bi bi-plus-circle"></i>
            {{ $t('Create New') }}
          </router-link>
        </div>
      </header>

      <div v-if="!requestIsOngoing" class="loader" aria-label="{{ $t('Loading') }}">
        <span></span><span></span><span></span>
      </div>

      <div v-else class="table-wrapper">
        <table>
          <thead>
          <tr>
            <th>Id</th>
            <th>{{ $t('Status') }}</th>
            <th>{{ $t('Quantity') }}</th>
            <th>{{ $t('Action Type') }}</th>
            <th>{{ $t('Reason') }}</th>
            <th>{{ $t('Supplier') }}</th>
            <th>{{ $t('Product') }}</th>
            <th>{{ $t('StorageRoom') }}</th>
            <th class="text-center" colspan="2">{{ $t('Actions') }}</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="item in filteredData" :key="item.id">
            <td>{{ item.id }}</td>
            <td><span :class="['badge', item.status.toLowerCase()]">{{ item.status }}</span></td>
            <td>{{ item.quantity }}</td>
            <td>{{ item.actionTypeName }}</td>
            <td class="reason-cell">{{ item.reasonDescription }}</td>
            <td>{{ item.supplierName }}</td>
            <td>{{ item.productName }}</td>
            <td>{{ item.storageRoomName }}</td>
            <td class="action-buttons">
              <button @click="updateStatus(item.id, 'Accepted')" class="btn-success" :aria-label="$t('Accept')">
                <i class="bi bi-check-circle"></i>
              </button>
            </td>
            <td class="action-buttons">
              <button @click="updateStatus(item.id, 'Declined')" class="btn-danger" :aria-label="$t('Decline')">
                <i class="bi bi-x-circle"></i>
              </button>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
    </section>
  </main>
</template>

<style scoped>
.request-page {
  display: flex;
  justify-content: center;
  padding: 2rem;
  color: #f4f4f4;
}

.card {
  width: 100%;
  max-width: 1600px;
  background: rgba(26, 26, 26, 0.95);
  backdrop-filter: blur(6px);
  border: 1px solid orange;
  border-radius: 12px;
  box-shadow: 0 0 20px rgba(255, 165, 0, 0.25);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  position: relative;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  padding: 1rem 2rem;
  background-color: rgba(43, 42, 42, 0.75);
  backdrop-filter: blur(4px);
  border-bottom: 1px solid orange;
}

.card-header h1 {
  margin: 0;
  font-size: 1.8rem;
  color: orange;
}

.card-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.filter-group select {
  background-color: #2a2a2a;
  color: white;
  border: 1px solid orange;
  padding: 0.4rem 0.8rem;
  border-radius: 12px;
  font-weight: 500;
  appearance: none;
  cursor: pointer;
}

.filter-group select option {
  background-color: #1a1a1a;
  color: white;
}

.btn-primary {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: orange;
  color: #000;
  padding: 0.5rem 1rem;
  border-radius: 12px;
  font-weight: 600;
  transition: background 0.3s;
  text-decoration: none;
}

.btn-primary:hover {
  background: #ffaa33;
}

.btn-success,
.btn-danger {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 36px;
  height: 36px;
  border-radius: 12px;
  border: none;
  cursor: pointer;
  color: white;
  font-size: 1.1rem;
  transition: transform 0.2s ease;
}

.btn-success { background: #28a745; }
.btn-danger  { background: #dc3545; }

.btn-success:hover,
.btn-danger:hover {
  transform: scale(1.05);
}

.table-wrapper {
  overflow-x: auto;
}

table {
  width: 100%;
  min-width: 900px;
  border-collapse: collapse;
  font-size: 0.95rem;
  color: #f4f4f4;
}

th,
td {
  padding: 0.75rem 1rem;
  border-bottom: 1px solid #333;
  text-align: center; /* tee kõik keskseks */
  vertical-align: middle;
  white-space: nowrap;
}

th {
  background: orange;
  color: black;
  border-bottom: 2px solid #1a1a1a;
}

.badge {
  min-width: 80px;
  display: inline-block;
  text-align: center;
}

tbody tr:nth-child(even) {
  background: rgba(255, 255, 255, 0.03);
}

tbody tr:hover {
  background: #2a2a2a;
}

.text-center {
  text-align: center;
}

.badge {
  display: inline-block;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.875rem;
  font-weight: bold;
  text-transform: capitalize;
  text-shadow: 0 0 2px black;
}

.badge.accepted { background: #28a745; color: white; }
.badge.declined { background: #dc3545; color: white; }
.badge.pending  { background: orange; color: black; }

.loader {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
  padding: 2rem;
}

.loader span {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background: orange;
  animation: bounce 0.6s infinite alternate;
}

.loader span:nth-child(2) { animation-delay: 0.2s; }
.loader span:nth-child(3) { animation-delay: 0.4s; }

@keyframes bounce {
  from { transform: translateY(0); opacity: 0.7; }
  to   { transform: translateY(-12px); opacity: 1; }
}

@media (max-width: 768px) {
  th:nth-child(5), td:nth-child(5),
  th:nth-child(8), td:nth-child(8) {
    display: none;
  }

  .card-header {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>

