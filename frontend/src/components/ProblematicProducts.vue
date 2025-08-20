<script setup lang="ts">
import { onMounted, ref } from 'vue'
import {ActionService} from "@/services/mvcServices/ActionService.ts";

// ---------------- Services ----------------
const service = new ActionService();

// ---------------- Entities ----------------
interface ProductStat {
  productId: string
  productName: string
  removeQuantity: number
  productUnit: string
  productVolume: number | null
  productVolumeUnit: string | null
}
const products = ref<ProductStat[]>([])

// ---------------- Fetch ----------------
const fetchProblematicProducts = async () => {
  try {
    products.value = await service.getTopRemovedProducts()
  } catch (error) {
    console.error('Failed to fetch problematic products:', error)
  }
}

onMounted(fetchProblematicProducts)
</script>

<template>
  <section
    class="h-full rounded-2xl border border-white/10 bg-neutral-900/70 p-5 sm:p-6 backdrop-blur-xl flex flex-col"
  >
    <!-- Header -->
    <header class="flex items-center justify-between">
      <h2 class="text-lg sm:text-xl font-semibold text-neutral-100 flex items-center gap-2">
        <span
          class="inline-flex h-6 w-6 items-center justify-center rounded-md bg-rose-500/15 ring-1 ring-rose-400/30 text-[13px]"
          aria-hidden="true"
        >🏷️</span>
        {{ $t('Most removed products') }}
      </h2>

      <span
        class="inline-flex h-8 items-center rounded-full border border-white/10 px-3 text-xs text-neutral-300"
        :aria-label="$t('Count')"
      >
        {{ products.length }}
      </span>
    </header>

    <div class="mt-3 h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent"></div>

    <!-- Content -->
    <div class="mt-4 flex-1">
      <ol
        v-if="products.length"
        class="space-y-1.5 max-h-56 overflow-y-auto pr-1"
        role="list"
        aria-live="polite"
      >
        <li
          v-for="(p, idx) in products"
          :key="p.productId"
          class="group grid grid-cols-[auto,1fr,auto] items-center gap-3 rounded-xl border border-white/5 bg-white/5 px-3 py-2.5
                 hover:bg-white/10 hover:border-white/10 transition-colors"
        >
          <!-- rank -->
          <div
            class="shrink-0 h-7 w-7 grid place-items-center rounded-lg bg-white/5 text-xs font-semibold text-neutral-300 ring-1 ring-white/10"
            :aria-label="$t('Rank') + ' ' + (idx + 1)"
          >
            {{ idx + 1 }}
          </div>

          <!-- Name + Progress -->
          <div class="min-w-0">
            <p class="truncate text-neutral-100 font-medium" :title="p.productName">
              {{ p.productName }}
            </p>

            <div class="mt-1 h-2 w-full rounded-full bg-white/5 ring-1 ring-white/10 overflow-hidden" role="progressbar"
                 :aria-valuenow="p.removeQuantity"
                 aria-valuemin="0"
                 :aria-valuemax="Math.max(...products.map(x => x.removeQuantity)) || 1">
              <div
                class="h-full rounded-full bg-rose-400/60"
                :style="{
                  width:
                    ((p.removeQuantity / (Math.max(...products.map(x => x.removeQuantity)) || 1)) * 100).toFixed(0) + '%'
                }"
              ></div>
            </div>

            <p class="mt-1 text-[11px] text-neutral-400">
              {{ $t('Discarded most often') }}
            </p>
          </div>

          <span
            class="shrink-0 rounded-lg px-2.5 py-1 text-xs font-semibold
                   bg-rose-500/10 text-rose-300 ring-1 ring-rose-400/30"
          >
            <template v-if="p.productUnit === 'tk' && p.productVolume && p.productVolumeUnit">
              {{
                (Number(p.removeQuantity) * Number(p.productVolume)).toLocaleString()
              }} {{ p.productVolumeUnit }}
            </template>
            <template v-else-if="p.productUnit">
              {{ p.removeQuantity }} {{ p.productUnit }}
            </template>
            <template v-else>
              {{ p.removeQuantity }} —
            </template>
          </span>
        </li>
      </ol>

      <div v-else class="text-center py-10">
        <div class="mx-auto mb-2 h-10 w-10 grid place-items-center rounded-xl bg-white/5 ring-1 ring-white/10">😶‍🌫️</div>
        <p class="text-neutral-400 italic">
          {{ $t('No problematic products found') }}
        </p>
      </div>
    </div>
  </section>
</template>
