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
  <div
    class="h-full rounded-2xl border border-white/10 bg-neutral-900/70 p-5 sm:p-6 backdrop-blur-xl flex flex-col"
  >
    <!-- header -->
    <div class="flex items-center justify-between">
      <h2 class="text-lg sm:text-xl font-semibold text-neutral-100 flex items-center gap-2">
        <span
          class="inline-flex h-6 w-6 items-center justify-center rounded-md bg-amber-500/15 ring-1 ring-amber-400/30 text-sm"
        >📦</span>
        {{ $t('Pending discards') }}
      </h2>
      <span
        class="inline-flex h-8 items-center rounded-full border border-white/10 px-3 text-xs text-neutral-300"
      >
        {{ filteredData.length }}
      </span>
    </div>

    <div
      class="mt-3 h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent"
    ></div>

    <!-- list -->
    <div class="mt-4 flex-1">
      <ul
        v-if="filteredData.length"
        class="space-y-1.5 max-h-56 overflow-y-auto pr-1"
      >
        <li
          v-for="a in filteredData.slice(0, 10)"
          :key="a.id"
          class="group grid grid-cols-[1fr,auto] items-center gap-3 rounded-xl border border-white/5 bg-white/5 px-3 py-2.5
                 hover:bg-white/10 hover:border-white/10 transition-colors"
        >
          <!-- product + action -->
          <div class="min-w-0">
            <p
              class="truncate text-neutral-100 font-medium"
              :title="a.productName"
            >
              {{ a.productName }}
            </p>
            <p
              class="truncate text-xs text-neutral-400"
              :title="a.actionTypeName"
            >
              {{ a.actionTypeName }}
            </p>
          </div>

          <!-- qty -->
          <span
            class="shrink-0 rounded-lg px-2.5 py-1 text-xs font-semibold
                   bg-amber-500/10 text-amber-300 ring-1 ring-amber-400/30"
          >
            {{ a.quantity }}
          </span>
        </li>
      </ul>

      <!-- empty -->
      <div v-else class="text-center py-10">
        <div
          class="mx-auto mb-2 h-10 w-10 grid place-items-center rounded-xl bg-white/5 ring-1 ring-white/10"
        >📭</div>
        <p class="text-neutral-400 italic">
          {{ $t('No pending actions') }}
        </p>
      </div>
    </div>

    <!-- cta -->
    <div class="pt-4 flex justify-end">
      <button
        @click="goToPendingPage"
        class="inline-flex items-center rounded-xl border border-cyan-400/30 bg-cyan-400/10 px-4 py-2 text-sm font-medium text-cyan-200 hover:bg-cyan-400/15 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition-colors"
      >
        {{ $t('View All') }}
      </button>
    </div>
  </div>
</template>
