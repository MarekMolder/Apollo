<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService'
import type { IStorageRoomEnriched } from '@/domain/logic/IStorageRoomEnriched.ts'

// Services
const storageRoomService = new StorageRoomService()

// Entity's
const data = ref<IStorageRoomEnriched[]>([])

// Router
const router = useRouter()

// Search engine
const searchQuery = ref('')

// Get storageRooms
const fetchPageData = async () => {
  try {
    const result = await storageRoomService.getEnrichedStorageRooms();
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching storageRooms:", error);
  }
};

onMounted(fetchPageData);

// Route to monthlyStatistics view
const goToCurrentStock = (storageRoomId: string) => {
  router.push(`/monthlyStatistics/${storageRoomId}`);
};

const filteredStorageRooms = computed(() =>
  data.value.filter((storageRoom) => {
      return storageRoom.name.toLowerCase().includes(searchQuery.value.toLowerCase());
  })
);
</script>

<template>
  <main class="p-8 text-white font-['Inter',sans-serif] bg-transparent max-w-screen-xl mx-auto">
    <!-- Header -->
    <section class="mb-8 text-center pb-3">
      <h1
        class="text-3xl sm:text-4xl font-extrabold text-[#ffaa33] m-0 drop-shadow-[0_0_12px_rgba(255,170,51,0.25)]"
      >
        🎬 Storage Rooms
      </h1>
      <p class="text-lg text-[#ccc] mt-2 pb-2">
        Visual overview of all Storage rooms and their locations
      </p>

      <!-- Search Input -->
      <div class="mt-6 flex justify-center">
        <input
          v-model="searchQuery"
          class="w-full max-w-[460px] px-5 py-3 text-base rounded-[16px] bg-[rgba(50,50,50,0.65)] text-white placeholder:text-[#ccc] shadow-[0_0_10px_rgba(255,170,51,0.1)] border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          placeholder="Search inventory by name..."
          type="text"
        />
      </div>
    </section>

    <!-- Storage Room Cards -->
    <div class="flex flex-col gap-8">
      <div
        v-for="storageRoom in filteredStorageRooms"
        :key="storageRoom.id"
        class="bg-[rgba(25,25,25,0.4)] rounded-[16px] p-8 backdrop-blur-xl shadow-[inset_0_0_20px_rgba(255,165,0,0.03),_0_8px_24px_rgba(0,0,0,0.4)] hover:shadow-[0_10px_30px_rgba(255,170,51,0.1),_0_0_0_1px_rgba(255,170,51,0.2)] hover:-translate-y-0.5 hover:border-[#ffaa33] duration-300 border-1 border-neutral-700 transition"
      >
        <div>
          <h2 class="text-2xl text-[#ffaa33] font-semibold m-0">{{ storageRoom.name }}</h2>
          <p class="text-sm text-[#bbb] mt-2">📍 {{ storageRoom.fullAddress }}</p>
          <button
            class="mt-4 bg-gradient-to-r from-[#ffaa33] to-[#ff8c00] text-black px-4 py-2 rounded-lg font-bold text-sm hover:from-[#ffc266] hover:to-[#ffa726] transition"
            @click="goToCurrentStock(storageRoom.id)"
          >
            View Stock
          </button>
        </div>
      </div>
    </div>
  </main>
</template>
