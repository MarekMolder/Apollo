<script setup lang="ts">
import {computed, onMounted, ref} from "vue";
import { useRouter } from "vue-router";
import { StorageRoomService } from "@/services/mvcServices/StorageRoomService";
import type {IStorageRoomEnriched} from "@/domain/logic/IStorageRoomEnriched.ts";

const router = useRouter();
const storageRoomService = new StorageRoomService();
const data = ref<IStorageRoomEnriched[]>([]);

const searchQuery = ref("");

const fetchPageData = async () => {
  try {
    const result = await storageRoomService.getEnrichedStorageRooms();
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching storageRooms:", error);
  }
};

onMounted(fetchPageData);

const goToCurrentStock = (storageRoomId: string) => {
  router.push(`/monthlyStatistics/${storageRoomId}`);
};

const filteredStorageRooms= computed(() =>
  data.value.filter((storageRoom) => {
      return storageRoom.name.toLowerCase().includes(searchQuery.value.toLowerCase());
  })
);
</script>


<template>
  <main class="inventory-wrapper">
    <section class="inventory-header">
      <h1 class="page-title">🎬Storage Rooms</h1>
      <p class="subtitle">Visual overview of all Storage rooms and their locations</p>

      <div class="search-container">
        <input
          v-model="searchQuery"
          class="search-box"
          placeholder="Search inventory by name..."
          type="text"
        />
      </div>
    </section>

    <div class="inventory-list">
      <div
        class="inventory-card"
        v-for="storageRoom in filteredStorageRooms"
        :key="storageRoom.id"
      >
        <div class="inventory-info">
          <h2>{{ storageRoom.name }}</h2>
          <p class="address">📍 {{ storageRoom.fullAddress }}</p>
          <button class="view-button" @click="goToCurrentStock(storageRoom.id)">
            View Stock
          </button>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.inventory-wrapper {
  padding: 2rem;
  font-family: 'Inter', sans-serif;
  color: white;
  background: transparent;
}

.inventory-header {
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2.8rem;
  font-weight: 800;
  color: #ffaa33;
  margin: 0;
  text-shadow: 0 0 12px rgba(255, 170, 51, 0.25);
}

.subtitle {
  font-size: 1.2rem;
  color: #ccc;
  margin-top: 0.5rem;
}

.search-container {
  margin-top: 1.5rem;
  display: flex;
  justify-content: center;
}

.search-box {
  padding: 0.75rem 1.25rem;
  font-size: 1rem;
  border-radius: 14px;
  border: 1px solid #ffaa33;
  background-color: rgba(50, 50, 50, 0.65);
  color: white;
  width: 100%;
  max-width: 460px;
  transition: all 0.25s ease;
  box-shadow: 0 0 10px rgba(255, 170, 51, 0.1);
}

.search-box:focus {
  outline: none;
  border-color: #ffc266;
  box-shadow: 0 0 12px rgba(255, 170, 51, 0.3);
}

.search-box::placeholder {
  color: #ccc;
}

.inventory-list {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.inventory-card {
  background: rgba(25, 25, 25, 0.4);
  border-radius: 18px;
  padding: 2rem;
  backdrop-filter: blur(12px);
  border: 1px solid rgba(255, 255, 255, 0.05);
  box-shadow:
    inset 0 0 20px rgba(255, 165, 0, 0.03),
    0 8px 24px rgba(0, 0, 0, 0.4);
  transition: all 0.3s ease;
}

.inventory-card:hover {
  box-shadow:
    0 10px 30px rgba(255, 170, 51, 0.1),
    0 0 0 1px rgba(255, 170, 51, 0.2);
  transform: translateY(-2px);
  border-color: #ffaa33;
}

.inventory-info h2 {
  margin: 0;
  font-size: 1.8rem;
  color: #ffaa33;
}

.address {
  color: #bbb;
  font-size: 0.95rem;
  margin-top: 0.4rem;
}

.storage-room-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
  gap: 1.5rem;
  margin-top: 1.5rem;
}

.storage-room-card {
  background: rgba(30, 30, 30, 0.7);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 14px;
  padding: 1rem;
  box-shadow: 0 4px 12px rgba(255, 170, 51, 0.1);
  transition: transform 0.2s ease;
}

.storage-room-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 6px 20px rgba(255, 170, 51, 0.2);
}

.room-info h3 {
  color: #ffaa33;
  margin: 0 0 0.3rem 0;
  font-size: 1.1rem;
}

.room-info p {
  font-size: 0.9rem;
  color: #ccc;
  margin-bottom: 0.6rem;
}

.view-button {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 10px;
  font-weight: bold;
  font-size: 0.9rem;
  cursor: pointer;
  transition: background 0.2s ease;
}

.view-button:hover {
  background: linear-gradient(to right, #ffc266, #ffa726);
}
</style>


