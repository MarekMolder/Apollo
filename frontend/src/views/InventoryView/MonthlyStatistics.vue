<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRoute } from "vue-router";
import { MonthlyStatisticsService } from "@/services/mvcServices/MonthlyStatisticsService";
import type { IMonthlyStatistics } from "@/domain/logic/IMonthlyStatistics";
import type {IMonthlyStatisticsEnriched} from "@/domain/logic/IMonthlyStatisticsEnriched.ts";

const route = useRoute();
const storageRoomId = route.params.storageRoomId as string;

const service = new MonthlyStatisticsService();
const data = ref<IMonthlyStatisticsEnriched[]>([]);

const selectedYear = ref(new Date().getFullYear());
const selectedMonth = ref(new Date().getMonth() + 1);

const yearOptions = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);
const monthOptions = Array.from({ length: 12 }, (_, i) => i + 1);

const filteredData = computed(() =>
  data.value.filter(
    (x) => x.year === selectedYear.value && x.month === selectedMonth.value
  )
);

onMounted(async () => {
  const result = await service.getEnrichedMonthlyStatistics();
  data.value = (result.data || []).filter(item => item.storageRoomId === storageRoomId);
});
</script>

<template>
  <main class="product-wrapper">
    <section class="product-header">
      <div class="product-header-left">
        <h1 class="page-title">📊 Monthly Statistics</h1>
        <p class="subtitle">Removed product quantities per month</p>
      </div>
    </section>

    <div class="filter-bar">
      <div class="filter-controls">
        <select v-model="selectedYear" class="search-box">
          <option v-for="year in yearOptions" :key="year" :value="year">{{ year }}</option>
        </select>
        <select v-model="selectedMonth" class="search-box">
          <option v-for="month in monthOptions" :key="month" :value="month">{{ month }}</option>
        </select>
      </div>
    </div>

    <div class="table-scroll-container">
      <table class="table table-striped table-bordered">
        <thead>
        <tr>
          <th>Product</th>
          <th>Product Code</th>
          <th>Product Unit</th>
          <th>Removed Quantity</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in filteredData" :key="item.id">
          <td>{{ item.productName }}</td>
          <td>{{ item.productCode }}</td>
          <td>{{ item.productUnit }}</td>
          <td>{{ item.totalRemovedQuantity }}</td>
        </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>

<style scoped>
.product-wrapper {
  height: 100%;
  display: flex;
  flex-direction: column;
  padding: 2rem;
  box-sizing: border-box;
  max-width: 1600px;
  margin: 0 auto;
  font-family: 'Inter', 'Segoe UI', sans-serif;
  color: white;
  background: transparent;
}

.product-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  flex-wrap: wrap;
}

.product-header-left {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.page-title {
  font-size: 2.6rem;
  font-weight: 800;
  color: #ffaa33;
  text-shadow: 0 0 10px rgba(255, 170, 51, 0.25);
  margin: 0;
}

.subtitle {
  font-size: 1rem;
  color: #bbb;
  margin: 0;
  opacity: 0.85;
}

.filter-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
  background: rgba(30, 30, 30, 0.6);
  backdrop-filter: blur(8px);
  padding: 1rem 1.5rem;
  border-radius: 16px;
  box-shadow: 0 0 12px rgba(255, 170, 51, 0.05);
}

.filter-controls {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.search-box {
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border-radius: 12px;
  border: 1px solid #ffaa33;
  background-color: rgba(43, 43, 43, 0.5);
  color: white;
  min-width: 220px;
  transition: all 0.2s ease;
}

.search-box:focus {
  outline: none;
  border-color: #ffc266;
  background-color: rgb(43, 43, 43);
}

.search-box::placeholder {
  color: #ccc;
}

.create-link {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  padding: 0.6rem 1.4rem;
  border-radius: 12px;
  font-weight: 700;
  text-decoration: none;
  box-shadow: 0 2px 6px rgba(255, 165, 0, 0.2);
  transition: all 0.2s ease;
}

.create-link:hover {
  background: linear-gradient(to right, #ffc56e, #ffa726);
  box-shadow: 0 3px 10px rgba(255, 165, 0, 0.3);
}

.table-scroll-container {
  flex-grow: 1;
  overflow-y: auto;
  padding: 1.2rem;
  margin-top: 1rem;
  border-radius: 16px;
  background: rgba(20, 20, 20, 0.5);
  backdrop-filter: blur(6px);
  box-shadow: inset 0 0 20px rgba(255, 165, 0, 0.05);
}

.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.product-card {
  background: rgba(45, 45, 45, 0.6);
  border-radius: 14px;
  padding: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.05);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.product-card:hover {
  box-shadow: 0 6px 16px rgba(255, 170, 51, 0.1);
  transform: translateY(-3px);
  border-color: #ffaa33;
}

.product-info h3 {
  color: #ffaa33;
  font-size: 1.1rem;
  margin-bottom: 0.4rem;
}

.product-info p {
  color: #ccc;
  font-size: 0.95rem;
  margin: 0.1rem 0;
}

.view-button {
  margin-top: 1rem;
  padding: 0.4rem 1rem;
  border: none;
  border-radius: 8px;
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-weight: bold;
  font-size: 0.9rem;
  cursor: pointer;
  transition: background 0.2s ease;
}

.view-button:hover {
  background-color: #ffc266;
}

.button-group {
  display: flex;
  gap: 0.5rem;
  margin-top: 0.5rem;
}

@media (max-width: 768px) {
  .product-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .filter-bar {
    flex-direction: column;
  }

  .product-list {
    grid-template-columns: 1fr;
  }
}

.drawer-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: flex-end;
  z-index: 999;
}

.drawer {
  width: 420px;
  background: linear-gradient(to bottom, #1e1e1e, #121212);
  color: white;
  padding: 2rem;
  overflow-y: auto;
  height: 100%;
  box-shadow: -8px 0 20px rgba(0, 0, 0, 0.5);
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
  border-left: 1px solid rgba(255, 255, 255, 0.05);
}

.drawer input {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  border: none;
  background: rgba(60, 60, 60, 0.7);
  color: white;
  font-size: 1rem;
  transition: all 0.2s;
}

.drawer input:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}

.drawer-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1.5rem;
}

.update-btn {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-size: 0.95rem;
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  font-weight: 700;
  border: none;
  cursor: pointer;
  box-shadow: 0 3px 8px rgba(0, 0, 0, 0.2);
}

.cancel-btn {
  background: #333;
  color: white;
  font-size: 0.95rem;
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  cursor: pointer;
  transition: all 0.2s ease;
}

.cancel-btn:hover {
  background: #444;
}

.text-danger {
  color: #ff4d4d;
  font-weight: bold;
  text-align: center;
}

.text-success {
  color: #28a745;
  font-weight: bold;
  text-align: center;
}

a.view-button {
  text-decoration: none;
}

.drawer select {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  border: none;
  background: rgba(60, 60, 60, 0.7);
  color: white;
  font-size: 1rem;
  appearance: none;
  background-image: url('data:image/svg+xml;charset=US-ASCII,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 4 5"><path fill="white" d="M2 0L0 2h4zm0 5L0 3h4z"/></svg>');
  background-repeat: no-repeat;
  background-position: right 0.75rem center;
  background-size: 0.65rem auto;
  transition: all 0.2s;
}

.drawer select:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}
</style>
