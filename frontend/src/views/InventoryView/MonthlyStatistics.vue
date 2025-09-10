<script setup lang="ts">
import { ref, onMounted, computed, watch } from "vue";
import { useRoute } from "vue-router";
import { MonthlyStatisticsService } from "@/services/mvcServices/MonthlyStatisticsService";
import type { IMonthlyStatisticsEnriched } from "@/domain/logic/IMonthlyStatisticsEnriched.ts";
import { useSidebarStore } from '@/stores/sidebarStore';
import * as XLSX from "xlsx";

// ---------------- Services ----------------
const service = new MonthlyStatisticsService();

// ---------------- Entities ----------------
const data = ref<IMonthlyStatisticsEnriched[]>([]);

// ---------------- Store && Router ----------------
const route = useRoute();
const sidebarStore = useSidebarStore();
const storageRoomId = route.params.storageRoomId as string;

// ---------------- Filters ----------------
const selectedYear = ref(new Date().getFullYear());
const selectedMonth = ref(new Date().getMonth() + 1);
const yearOptions = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);
const monthOptions = Array.from({ length: 12 }, (_, i) => i + 1);
const nameQuery = ref("");
const codeQuery = ref("");
const selectedCategoryId = ref<"All" | string>("All");
const selectedDay = ref<"All" | number>("All");

// ---------------- Unit conversion ----------------
const selectedUnits = ref<Record<string, string>>({});
const convertedQuantities = ref<Record<string, string>>({});
const availableUnits = ["g", "kg", "mg", "ml", "l", "cl"];
const selectedVolumeUnits = ref<Record<string, string>>({});
const convertedVolumes = ref<Record<string, string>>({});

// ---------------- Helpers ----------------
const showHelp = ref(false);
const normalize = (s: string) => s.trim().toLowerCase();

// ---------------- Excel columns ----------------
const exportColumns = [
  {
    key: "productName",
    label: "Name",
    get: (x: IMonthlyStatisticsEnriched) => x.productName ?? "",
  },
  {
    key: "productCode",
    label: "Code",
    get: (x: IMonthlyStatisticsEnriched) => x.productCode ?? "",
  },
  {
    key: "productCategoryName",
    label: "Category",
    get: (x: IMonthlyStatisticsEnriched) => x.productCategoryName ?? "",
  },
  {
    key: "normalizedAmount",
    label: "Removed Amount (normalized)",
    get: (x: IMonthlyStatisticsEnriched) => getUnifiedAmount(x),
  },
  {
    key: "productUnit",
    label: "Unit (raw)",
    get: (x: IMonthlyStatisticsEnriched) => x.productUnit ?? "",
  },
  {
    key: "totalRemovedQuantity",
    label: "Removed Quantity (raw)",
    get: (x: IMonthlyStatisticsEnriched) => x.totalRemovedQuantity,
  },
  {
    key: "removedVolume",
    label: "Removed Volume (raw)",
    get: (x: IMonthlyStatisticsEnriched) => {
      const vol = calcRemovedVolume(x);
      return vol ? `${vol.value} ${vol.unit ?? ""}`.trim() : "";
    },
  },
];

const selectedExportKeys = ref<string[]>([
  "productName",
  "productCode",
  "productCategoryName",
  "normalizedAmount",
]);

// ---------------- Fetch ----------------
onMounted(async () => {
  const result = await service.getByStorageRoomId(storageRoomId);
  data.value = result.data || [];

  for (const item of data.value) {
    selectedUnits.value[item.id] = item.productUnit;
    convertedQuantities.value[item.id] = `${item.totalRemovedQuantity} ${item.productUnit}`;

    if (item.productUnit === "tk" && item.productVolumeUnit) {
      selectedVolumeUnits.value[item.id] = item.productVolumeUnit;
      const vol = calcRemovedVolume(item);
      if (vol) convertedVolumes.value[item.id] = `${vol.value} ${item.productVolumeUnit}`;
    }
  }
});

async function fetchConvertedQuantity(id: string, targetUnit: string) {
  try {
    const result = await service.getConvertedQuantity(id, targetUnit);
    convertedQuantities.value[id] = result;
  } catch {
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

function getUnifiedAmount(item: IMonthlyStatisticsEnriched): string {
  if (item.productUnit === "tk") {
    const cached = convertedVolumes.value[item.id];
    if (cached) return cached;

    const vol = calcRemovedVolume(item);
    if (!vol) return "";
    const unit = selectedVolumeUnits.value[item.id] ?? vol.unit ?? "";
    return `${vol.value} ${unit}`.trim();
  }

  const cachedQ = convertedQuantities.value[item.id];
  if (cachedQ) return cachedQ;

  const unit = selectedUnits.value[item.id] ?? item.productUnit;
  return `${item.totalRemovedQuantity} ${unit}`.trim();
}

// ---------------- Search engine filtered monthlyStatistics ----------------
const categoryOptions = computed(() => {
  const map = new Map<string, string>();
  for (const it of data.value ?? []) {
    if (it.productCategoryId && it.productCategoryName) {
      const id = String(it.productCategoryId);
      if (!map.has(id)) map.set(id, it.productCategoryName);
    }
  }
  return Array.from(map, ([id, name]) => ({ id, name }))
    .sort((a, b) => a.name.localeCompare(b.name));
});

const dayOptions = computed<number[]>(() => {
  const days = new Set<number>();
  for (const it of data.value ?? []) {
    if (it.year === selectedYear.value && it.month === selectedMonth.value) {
      const d = it.day;
      if (typeof d === "number" && !Number.isNaN(d) && d > 0) days.add(d);
    }
  }
  return Array.from(days).sort((a, b) => a - b);
});

const filteredData = computed(() => {
  let items = (data.value ?? []).filter(
    (x) => x.year === selectedYear.value && x.month === selectedMonth.value
  );

  // name filter
  if (nameQuery.value.trim()) {
    const q = normalize(nameQuery.value);
    items = items.filter((i) => normalize(i.productName ?? "").includes(q));
  }

  // code filter
  if (codeQuery.value.trim()) {
    const q = normalize(codeQuery.value);
    items = items.filter((i) => normalize(i.productCode ?? "").includes(q));
  }

  // category filter by ID
  if (selectedCategoryId.value !== "All") {
    const sel = String(selectedCategoryId.value);
    items = items.filter((i) => String(i.productCategoryId) === sel);
  }

  // day filter
  if (selectedDay.value !== "All") {
    items = items.filter((i) => i.day === selectedDay.value);
  }

  return items;
});

// ---------------- Conversions ----------------
watch([selectedYear, selectedMonth], () => {
  selectedDay.value = "All";
});

function calcRemovedVolume(item: IMonthlyStatisticsEnriched) {
  if (item.productUnit !== "tk") return null;
  const vol = Number(item.productVolume) || 0;
  const qty = Number(item.totalRemovedQuantity) || 0;
  return { value: vol * qty, unit: item.productVolumeUnit };
}

// ---------------- Excel export ----------------
const exportToExcel = () => {
  if (!filteredData.value.length || !selectedExportKeys.value.length) return;

  const rows = filteredData.value.map((item) => {
    const row: Record<string, unknown> = {};
    for (const key of selectedExportKeys.value) {
      const col = exportColumns.find((c) => c.key === key)!;
      row[col.label] = col.get(item);
    }
    return row;
  });

  const ws = XLSX.utils.json_to_sheet(rows);
  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, "Monthly");

  const y = selectedYear.value;
  const m = String(selectedMonth.value).padStart(2, "0");
  XLSX.writeFile(wb, `monthly_statistics_${y}-${m}.xlsx`);
};
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-4 sm:p-6 lg:p-8 text-white max-w-screen-2xl',
      sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
    ]"
  >
    <!-- HEADER -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]
               relative inline-block"
      >
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          {{ $t('Monthly statistics') }}
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">{{ $t('Removed product quantities per month') }}</p>
    </section>

    <!-- Card container -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8
               bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
               border-1 border-neutral-700
               shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]"
      >
        <!-- Toolbar: Year / Month + filters -->
        <div class="mb-6 grid grid-cols-1 md:grid-cols-2 xl:grid-cols-6 gap-3 items-center">

          <!-- Year -->
          <div class="relative">
            <label class="sr-only">{{ $t('Year') }}</label>
            <select
              v-model="selectedYear"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9"
            >
              <option v-for="year in yearOptions" :key="year" :value="year">{{ year }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Month -->
          <div class="relative">
            <label class="sr-only">{{ $t('Month') }}</label>
            <select
              v-model="selectedMonth"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9"
            >
              <option v-for="month in monthOptions" :key="month" :value="month">{{ month }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Day filter -->
          <div class="relative">
            <label class="sr-only">{{ $t('Day') }}</label>
            <select
              v-model="selectedDay"
              :disabled="dayOptions.length === 0"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9
                     disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <option value="All">{{ $t('All days') }}</option>
              <option v-for="d in dayOptions" :key="d" :value="d">{{ d }}</option>
            </select>
            <i class="bi bi-clock-history absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Name filter -->
          <div class="relative">
            <label class="sr-only">{{ $t('Name') }}</label>
            <input
              v-model="nameQuery"
              type="text"
              placeholder="Filter by name"
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30"
            />
            <i class="bi bi-search absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Code filter -->
          <div class="relative">
            <label class="sr-only">{{ $t('Code') }}</label>
            <input
              v-model="codeQuery"
              type="text"
              placeholder="Filter by code"
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30"
            />
            <i class="bi bi-upc-scan absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Category filter -->
          <div class="relative">
            <label class="sr-only">{{ $t('Category') }}</label>
            <select
              v-model="selectedCategoryId"
              :disabled="categoryOptions.length === 0"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9
                     disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <option value="All">{{ $t('All categories') }}</option>
              <option v-for="cat in categoryOptions" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
            </select>
            <i class="bi bi-tags absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Excel export -->
          <div class="col-span-1 xl:col-span-6 flex flex-wrap items-center gap-3 mt-2">
            <div class="flex flex-wrap items-center gap-2">
              <span class="text-sm text-neutral-400">{{ $t('Columns') }}:</span>
              <div class="flex flex-wrap gap-2">
                <label
                  v-for="c in exportColumns"
                  :key="c.key"
                  class="inline-flex items-center gap-2 text-sm text-neutral-300 bg-neutral-900/70 border border-neutral-700 rounded-lg px-2 py-1"
                >
                  <input
                    type="checkbox"
                    :value="c.key"
                    v-model="selectedExportKeys"
                    class="accent-cyan-400"
                  />
                  {{ c.label }}
                </label>
              </div>
            </div>

            <button
              @click="exportToExcel"
              :disabled="!filteredData.length || !selectedExportKeys.length"
              class="ml-auto inline-flex items-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
           border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
           shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
           hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
           focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition
           disabled:opacity-50 disabled:cursor-not-allowed"
              title="Export visible rows to Excel"
            >
              <i class="bi bi-download"></i>
              {{ $t('Export Excel') }}
            </button>
          </div>

        </div>

        <!-- Table -->
        <div class="overflow-auto rounded-[12px] border-1 border-neutral-700">
          <table class="w-full text-base text-left">
            <thead class="sticky top-0 z-10 bg-neutral-900/80 backdrop-blur text-neutral-300">
            <tr>
              <th class="px-4 py-3">{{ $t('Product') }}</th>
              <th class="px-4 py-3 hidden md:table-cell">{{ $t('Code') }}</th>
              <th class="px-4 py-3">{{ $t('Category') }}</th>
              <th class="px-4 py-3">{{ $t('Unit') }}</th>
              <th class="px-4 py-3">{{ $t('Removed quantity') }}</th>
              <th class="px-4 py-3">{{ $t('Removed volume') }}</th>
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

              <!-- Category 🆕 -->
              <td class="px-4 py-3 align-middle text-neutral-300">
                <div
                  v-if="item.productCategoryName"
                  class="inline-block text-xs px-2 py-1 rounded-md border border-neutral-700 bg-neutral-900/60"
                >
                  {{ item.productCategoryName }}
                </div>
                <span v-else class="text-neutral-500">—</span>
              </td>

              <!-- Unit -->
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

              <!-- Removed Volume -->
              <td class="px-4 py-3 align-middle">
                <template v-if="calcRemovedVolume(item)">
                  <div class="flex items-center gap-3">
                      <span class="font-medium text-neutral-100">
                        {{ convertedVolumes[item.id] || '…' }}
                      </span>

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
                {{ $t('No data to display') }}
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>

    <!-- HELP BUTTON -->
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

    <!-- HELP MODAL -->
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
              {{ $t('How to use this page?') }}
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
              See vaade näitab valitud laoruumi <strong>kuiseid mahakantud koguseid</strong>.
              Filtreeri ülevalt aasta ja kuu järgi ning täpsusta tulemusi lisafilteritega.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Aasta & kuu & päeva:</strong> vali rippmenüüdest periood, mille andmeid soovid vaadata.
              </li>
              <li>
                <strong>Filtrid:</strong> <em>Filter by name</em> ja <em>Filter by code</em> otsivad vastavalt toote nime ja koodi järgi.
                <em>Category</em> piirab kategooria kaupa.
              </li>
              <li>
                <strong>Tabel:</strong> veerud näitavad toodet, koodi, kategooriat, ühikut ning eemaldatud kogust/mahtu vastavalt sinu valikutele.
              </li>
              <li>
                <strong>Ühikute vahetus (Quantity):</strong> kui toote ühik ei ole <code>tk</code>, saad veerus
                <em>Unit</em> valida sihtühiku (nt g, kg, ml, l). Vastav <em>Removed Quantity</em> teisendatakse automaatselt.
              </li>
              <li>
                <strong>Tükikaubad (Volume):</strong> kui toote ühik on <code>tk</code>, kuvatakse veerus
                <em>Removed Volume</em> kogumaht (tk × toote maht). Vali kõrval rippmenüüst soovitud mahuühik (nt ml, l),
                et näha mahtu selles ühikus.
              </li>
              <li>
                <strong>Excelisse eksport:</strong> vali <em>Columns</em> alt, millised veerud kaasata, ja vajuta
                <em>Export Excel</em>, et salvestada hetkel nähtavad read failiks <code>monthly_statistics_YYYY-MM.xlsx</code>.
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaali saad sulgeda taustale klõpsates või ülanurga <em>×</em> nupust. Enne uute kirjete lisamist kasuta otsingut,
              et vältida duplikaate.            </p>
          </div>
          </div>

          <!-- Footer -->
          <div class="mt-6 flex justify-end">
            <button
              @click="showHelp = false"
              class="inline-flex items-center justify-center rounded-xl border border-neutral-700
                 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200
                 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              {{ $t('Got it') }}
            </button>
          </div>
        </div>
    </transition>
  </main>
</template>
