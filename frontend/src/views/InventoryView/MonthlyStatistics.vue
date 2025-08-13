<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRoute } from "vue-router";
import { MonthlyStatisticsService } from "@/services/mvcServices/MonthlyStatisticsService";
import type {IMonthlyStatisticsEnriched} from "@/domain/logic/IMonthlyStatisticsEnriched.ts";

// Services
const service = new MonthlyStatisticsService();

// Entity's
const data = ref<IMonthlyStatisticsEnriched[]>([]);

// Route
const route = useRoute();
const storageRoomId = route.params.storageRoomId as string;

// Search engine
const selectedYear = ref(new Date().getFullYear());
const selectedMonth = ref(new Date().getMonth() + 1);
const yearOptions = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);
const monthOptions = Array.from({ length: 12 }, (_, i) => i + 1);

// Unit conversion
const selectedUnits = ref<Record<string, string>>({});
const convertedQuantities = ref<Record<string, string>>({});
const availableUnits = ["g", "kg", "mg", "ml", "l", "cl"];
const selectedVolumeUnits = ref<Record<string, string>>({});
const convertedVolumes = ref<Record<string, string>>({});

// Get monthlyStatistics
onMounted(async () => {
  const result = await service.getByStorageRoomId(storageRoomId);

  data.value = result.data || [];

  for (const item of data.value) {
    selectedUnits.value[item.id] = item.productUnit;
    convertedQuantities.value[item.id] = `${item.totalRemovedQuantity} ${item.productUnit}`;

    if (item.productUnit === "tk" && item.productVolumeUnit) {
      selectedVolumeUnits.value[item.id] = item.productVolumeUnit;
      convertedVolumes.value[item.id] = `${calcRemovedVolume(item)!.value} ${item.productVolumeUnit}`;
    }
  }
});

async function fetchConvertedQuantity(id: string, targetUnit: string) {
  try {
    const result = await service.getConvertedQuantity(id, targetUnit);
    convertedQuantities.value[id] = result;
  } catch (e) {
    convertedQuantities.value[id] = "❌";
  }
}

async function fetchConvertedVolume(id: string, targetUnit: string) {
  try {
    const result = await service.getConvertedVolume(id, targetUnit);
    convertedVolumes.value[id] = result;
  } catch {
    convertedVolumes.value[id] = "❌";
  }
}

// Search engine filtered monthlyStatistics
const filteredData = computed(() =>
  data.value.filter(
    (x) => x.year === selectedYear.value && x.month === selectedMonth.value
  )
);

function calcRemovedVolume(item: IMonthlyStatisticsEnriched) {
  if (item.productUnit !== 'tk') return null;
  const vol = Number(item.productVolume) || 0;
  const qty = Number(item.totalRemovedQuantity) || 0;
  return { value: vol * qty, unit: item.productVolumeUnit };
}
</script>

<template>
  <main class="p-6 sm:p-8 text-white font-['Inter',sans-serif] bg-transparent max-w-screen-2xl mx-auto">
    <!-- Header (nagu Requests) -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]
               relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          Monthly Statistics
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">Removed product quantities per month</p>
    </section>

    <!-- Kaart/Container -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8
               bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
               border-1 border-neutral-700
               shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]">

        <!-- Toolbar: Aasta / Kuu -->
        <div class="mb-6 grid grid-cols-1 sm:grid-cols-2 gap-3 items-center">
          <div class="relative">
            <label class="sr-only">Year</label>
            <select
              v-model="selectedYear"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9">
              <option v-for="year in yearOptions" :key="year" :value="year">{{ year }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <div class="relative">
            <label class="sr-only">Month</label>
            <select
              v-model="selectedMonth"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9">
              <option v-for="month in monthOptions" :key="month" :value="month">{{ month }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>
        </div>

        <!-- Tabel -->
        <div class="overflow-auto rounded-[12px] border-1 border-neutral-700">
          <table class="w-full text-base text-left">
            <thead class="sticky top-0 z-10 bg-neutral-900/80 backdrop-blur text-neutral-300">
            <tr>
              <th class="px-4 py-3">Product</th>
              <th class="px-4 py-3 hidden md:table-cell">Code</th>
              <th class="px-4 py-3">Unit</th>
              <th class="px-4 py-3">Removed Quantity</th>
              <th class="px-4 py-3">Removed Volume</th>
            </tr>
            </thead>
            <tbody>
            <tr
              v-for="item in filteredData"
              :key="item.id"
              class="border-t border-white/10 even:bg-white/5 hover:bg-white/10 transition"
            >
              <!-- Product -->
              <td class="px-4 py-3 align-middle">
                <div class="font-medium text-neutral-100">{{ item.productName }}</div>
              </td>

              <!-- Code -->
              <td class="px-4 py-3 align-middle hidden md:table-cell text-neutral-300">
                {{ item.productCode }}
              </td>

              <!-- Unit (valik tk mitte muudetav) -->
              <td class="px-4 py-3 align-middle">
                <template v-if="item.productUnit !== 'tk'">
                  <div class="relative inline-block">
                    <select
                      v-model="selectedUnits[item.id]"
                      @change="fetchConvertedQuantity(item.id, selectedUnits[item.id])"
                      class="h-9 text-base appearance-none rounded-lg border-1 border-neutral-700 bg-neutral-900/70 text-white
                      pl-3 pr-8 focus:outline-none focus:ring-2 focus:ring-cyan-400/25
                      focus:border-neutral-500 transition shadow-inner shadow-black/30"
                    >
                      <option v-for="unit in availableUnits" :key="unit" :value="unit">{{ unit }}</option>
                    </select>
                    <i class="bi bi-chevron-down pointer-events-none absolute right-2 top-1/2 -translate-y-1/2 text-neutral-400 text-xs"></i>
                  </div>
                </template>
                <template v-else>
                  <span class="inline-block rounded-md border-1 border-neutral-700 px-2 py-1 text-medium text-neutral-300 bg-neutral-900/70">tk</span>
                </template>
              </td>

              <!-- Removed Quantity -->
              <td class="px-4 py-3 align-middle">
                  <span class="text-neutral-100">
                    {{ convertedQuantities[item.id] || '…' }}
                  </span>
              </td>

              <!-- Removed Volume (ainult kui tk) -->
              <td class="px-4 py-3 align-middle">
                <template v-if="calcRemovedVolume(item)">
                  <div class="flex items-center gap-3">
                    <!-- Number vasakul -->
                    <span class="font-medium text-neutral-100">
                      {{ convertedVolumes[item.id] || '…' }}
                    </span>

                    <!-- Unit valik paremal -->
                    <div class="relative inline-block">
                      <select
                        v-model="selectedVolumeUnits[item.id]"
                        @change="fetchConvertedVolume(item.id, selectedVolumeUnits[item.id])"
                        class="h-9 appearance-none rounded-lg border-1 border-neutral-700 bg-neutral-900/70 text-white
                   pl-3 pr-8 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/25
                   focus:border-neutral-500 transition shadow-inner shadow-black/30"
                      >
                        <option v-for="unit in availableUnits" :key="unit" :value="unit">{{ unit }}</option>
                      </select>
                      <i class="bi bi-chevron-down pointer-events-none absolute right-2 top-1/2 -translate-y-1/2 text-neutral-400 text-xs"></i>
                    </div>
                  </div>
                </template>
                <template v-else>—</template>
              </td>
            </tr>

            <tr v-if="filteredData.length === 0">
              <td colspan="5" class="px-4 py-10 text-center text-neutral-400">
                No data to display
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  </main>
</template>
