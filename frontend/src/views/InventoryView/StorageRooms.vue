<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import {useRoute, useRouter} from "vue-router";
import type {IStorageRoom} from "@/domain/logic/IStorageRoom.ts";
import {StorageRoomService} from "@/services/mvcServices/StorageRoomService.ts";

const route = useRoute();
const router = useRouter();
const inventoryId = route.params.inventoryId as string;

const data = ref<IStorageRoom[]>([]);
const service = new StorageRoomService();

const requestIsOngoing = ref(false);



const fetchPageData = async () => {
  try {
    const result = await service.getByInventoryId(inventoryId);
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching storageRooms:", error);
  } finally {
    requestIsOngoing.value = true;
  }
};

onMounted(fetchPageData);

const goToCurrentStock = (storageRoomId: string) => {
  router.push(`/currentstock/${storageRoomId}`);
};

</script>

<template>
  <main class="product-wrapper">
    <h1 class="page-title">{{ $t('Storage Rooms') }}</h1>

    <div class="table-scroll-container">
      <div class="product-list">
        <div class="product-card" v-for="item in data" :key="item.id">
          <div class="product-picture">
            <img alt="Product" />
          </div>
          <div class="product-info">
            <h3>{{ item.name }}</h3>
            <p><strong>{{ $t('Location') }}:</strong> {{ item.location }}</p>
            <button class="view-button" @click="goToCurrentStock(item.id)">
              {{ $t('View Current Stock') }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.product-wrapper {
  padding: 2rem;
  font-family: Arial, sans-serif;
  background-color: #1a1a1a;
  color: white;
}

.page-title {
  text-align: center;
  font-size: 2rem;
  color: orange;
  margin-bottom: 1rem;
}

.table-scroll-container {
  max-height: 650px;
  overflow-y: auto;
  border: 1px solid #444;
  border-radius: 8px;
}

.filter-bar {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  flex-wrap: wrap;
  margin-bottom: 1.5rem;
}

.filter-controls {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.create-link {
  white-space: nowrap;
}

.search-box,
select {
  padding: 0.5rem;
  font-size: 1rem;
  border-radius: 6px;
  border: 2px solid orange;
  background-color: #2a2a2a;
  color: white;
}

.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 1.5rem;
}

.product-card {
  display: flex;
  align-items: center;
  background-color: #2a2a2a;
  border: 2px solid orange;
  border-radius: 12px;
  padding: 1rem;
  gap: 1rem;
  box-shadow: 0 0 8px rgba(255, 165, 0, 0.2);
}

.product-picture img {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background-color: #444;
}

.product-info {
  flex: 1;
}

.view-button {
  background-color: orange;
  color: white;
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.2s;
}

.view-button:hover {
  background-color: #ffaa33;
}

.create-link {
  background-color: orange;
  padding: 0.5rem 1rem;
  color: white;
  text-decoration: none;
  border-radius: 6px;
  font-weight: bold;
  transition: background 0.3s ease;
}

.create-link:hover {
  background-color: #ffaa33;
}
</style>
