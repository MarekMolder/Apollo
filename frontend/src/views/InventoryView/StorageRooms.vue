<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService'
import type { IStorageRoomEnriched } from '@/domain/logic/IStorageRoomEnriched.ts'
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();
const showHelp = ref(false);
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

const gridCols = computed(() => {
  const n = filteredStorageRooms.value.length
  if (n <= 1) return 'grid-cols-1'
  if (n === 2) return 'grid-cols-2'
  return 'grid-cols-3'
})
</script>

<template>
  <main
    :class="[
    'transition-all duration-300 p-4 sm:p-6 lg:p-8 text-white max-w-screen-2xl',
    sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
  ]"
  >
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]
               relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          Storage Rooms
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
    </section>

    <!-- Card container -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8
               bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
               border-1 border-neutral-700
               shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]">

        <!-- Search bar -->
        <div class="mb-6 flex items-center gap-2">
          <i class="bi bi-search text-neutral-400 hidden sm:inline"></i>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Search storage room..."
            class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                   px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                   focus:border-neutral-500 transition shadow-inner shadow-black/30"
          />
        </div>

        <!-- Grid of storage rooms -->
        <div class="grid gap-6" :class="gridCols">
          <div
            v-for="room in filteredStorageRooms"
            :key="room.id"
            class="rounded-xl p-5 bg-neutral-900/60 border border-neutral-700
           shadow-[0_4px_12px_rgba(0,0,0,0.3)]
           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15
           transition"
          >
            <!-- Nimi ja aadress -->
            <h2 class="text-2xl font-bold text-neutral-200">{{ room.name }}</h2>
            <p class="text-base text-neutral-400 mt-2">📍 {{ room.fullAddress }}</p>

            <!-- Nupp väiksem -->
            <div class="mt-6 flex justify-end">
              <button
                @click="goToCurrentStock(room.id)"
                class="inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
               border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent text-cyan-200
               shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_4px_12px_rgba(0,0,0,0.35)]
               hover:from-cyan-400/30 hover:via-cyan-300/20 hover:text-white
               focus:outline-none focus:ring-2 focus:ring-cyan-400/40 transition no-underline"
              >
                <i class="bi bi-box-seam text-base"></i>
                View Stock
              </button>
            </div>
          </div>
        </div>

        <!-- No results -->
        <div v-if="filteredStorageRooms.length === 0" class="text-center text-neutral-400 mt-8">
          No storage rooms found.
        </div>
      </div>
    </section>

    <!-- 🟣 FLOATING HELP BUTTON -->
    <button
      @click="showHelp = true"
      class="fixed z-[1100] bottom-6 right-6 w-12 h-12 rounded-full
         bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent
         border-1 border-neutral-700 text-neutral-100
         shadow-[0_6px_24px_rgba(0,0,0,0.45)]
         hover:from-cyan-500/30 hover:via-cyan-400/20
         backdrop-blur-sm transition focus:outline-none
         focus:ring-2 focus:ring-cyan-400/40"
      aria-label="Help & guide"
      title="Help"
    >
      <i class="bi bi-question-lg text-xl"></i>
    </button>

    <!-- 🟣 HELP MODAL -->
    <transition name="fade">
      <div
        v-if="showHelp"
        class="fixed inset-0 z-[1200] flex items-center justify-center bg-black/60 p-4"
        @click.self="showHelp = false"
      >
        <div
          class="w-full max-w-3xl rounded-2xl border border-white/10
             bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
             shadow-[0_20px_60px_rgba(0,0,0,0.6)]"
          role="dialog"
          aria-modal="true"
          aria-labelledby="help-title"
        >
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 id="help-title" class="text-2xl font-bold tracking-tight text-neutral-100">
              Kuidas seda lehte kasutada?
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                 border border-white/10 bg-white/5 text-neutral-300
                 hover:bg-white/10 hover:text-white focus:outline-none
                 focus:ring-2 focus:ring-white/15"
              @click="showHelp = false"
              title="Sulge"
              aria-label="Close help"
            >
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <!-- Body -->
          <div class="mt-5 space-y-4 text-neutral-200 leading-relaxed">
            <p>
              Sellel lehel saad <strong>otsida</strong>, <strong>luua</strong>, <strong>muuta</strong> ja
              <strong>kustutada</strong> tarnijaid ning vaadata, millised tooted on konkreetse tarnijaga seotud.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li><strong>Otsing:</strong> ülal vasakul “Search by name” filtreerib kaarte nime järgi.</li>
              <li><strong>Uus tarnija:</strong> klõpsa “New Supplier”, täida vorm ja salvesta.</li>
              <li><strong>Muuda:</strong> kaardil <em>Edit</em> avab vormi olemasoleva tarnija muutmiseks.</li>
              <li><strong>Tooted:</strong> <em>Products</em> näitab valitud tarnija tooteid.</li>
              <li><strong>Kustuta:</strong> prügikasti ikoon kaardi paremas ülanurgas.</li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaalid saab sulgeda ka klõpsates tumedal taustal või vajutades sulgemisnupule.
            </p>
          </div>

          <!-- Footer -->
          <div class="mt-6 flex justify-end">
            <button
              @click="showHelp = false"
              class="inline-flex items-center justify-center rounded-xl border border-neutral-700
                 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200
                 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              Sain aru
            </button>
          </div>
        </div>
      </div>
    </transition>

  </main>
</template>


<style scoped>
/* Ühtlane pehme border kõigile “kaardi” elementidele, kui vaja laiendada */
.card-like {
  @apply bg-white/[0.04] backdrop-blur-xl border border-white/10 rounded-2xl;
}

/* Kui tahad “primary grey” nuppu taaskasutada */
.btn-primary-grey {
  @apply inline-flex items-center justify-center rounded-xl
  bg-gradient-to-b from-neutral-800 to-neutral-700
  text-white font-semibold px-6 py-3 text-base
  border border-white/10 shadow-[0_0_8px_rgba(0,0,0,0.4)]
  hover:border-[#ffaa33] hover:shadow-[0_0_15px_rgba(255,170,51,0.4)]
  focus:outline-none focus:ring-2 focus:ring-[#ffaa33]/50 transition;
}
</style>

