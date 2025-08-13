<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import type { IResultObject } from '@/types/IResultObject'
import type { IActionEnriched } from '@/domain/logic/IActionEnriched.ts'
import { ActionService } from '@/services/mvcServices/ActionService.ts'

// Services
const service = new ActionService()

// Entity's
const data = reactive<IResultObject<IActionEnriched[]>>({ data: [], errors: [] })

// ??
const requestIsOngoing = ref(false)

// Search engine
const selectedStatus = ref<'All' | 'Accepted' | 'Declined' | 'Pending'>('All')

// Get actions
const fetchPageData = async () => {
  try {
    const result = await service.getEnrichedActions()
    data.data = result.data
    data.errors = result.errors
  } catch (error) {
    console.error('Error fetching data:', error)
  } finally {
    requestIsOngoing.value = true
  }
}

onMounted(fetchPageData)

// Search engine filtered suppliers
const filteredData = computed(() => {
  if (selectedStatus.value === 'All') return data.data
  return data.data!.filter((item) => item.status === selectedStatus.value)
})

// Accept / Deckline function
const editStatus = async (id: string, newStatus: 'Accepted' | 'Declined') => {
  try {
    const response = await service.patchStatus(id, newStatus)
    console.log(response.message)
    await fetchPageData()
  } catch (error) {
    console.error('Status update failed', error)
  }
}
</script>

<template>
  <main class="flex justify-center px-4 py-8 text-white">
    <section
      class="w-full max-w-7xl bg-[rgba(26,26,26,0.95)] backdrop-blur-sm rounded-[16px] shadow-[0_0_16px_rgba(255,165,0,0.2)] overflow-hidden"
    >
      <!-- Header -->
      <header
        class="rounded-t-[16px] flex flex-col md:flex-row justify-between items-start md:items-center gap-4 px-4 sm:px-6 py-4 bg-[rgba(43,42,42,0.75)] backdrop-blur"
      >
        <h1 class="text-2xl font-bold text-[#ffaa33] m-0">{{ $t('Requests') }}</h1>
        <div class="flex flex-wrap gap-4 items-center">
          <div class="flex items-center gap-2">
            <i class="bi bi-funnel"></i>
            <select
              v-model="selectedStatus"
              class="bg-[#2a2a2a] text-white rounded-xl px-3 py-1 font-medium cursor-pointer border border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
            >
              <option value="All">{{ $t('All') }}</option>
              <option value="Accepted">{{ $t('Accepted') }}</option>
              <option value="Declined">{{ $t('Declined') }}</option>
              <option value="Pending">{{ $t('Pending') }}</option>
            </select>
          </div>
          <router-link
            to="/createaction"
            class="flex items-center gap-2 bg-[#ffaa33] text-black px-4 py-2 rounded-xl font-semibold hover:bg-[#ffaa33] transition no-underline"
          >
            <i class="bi bi-plus-circle"></i>
            {{ $t('Create New') }}
          </router-link>
        </div>
      </header>

      <!-- Loading state -->
      <div v-if="!requestIsOngoing" class="flex justify-center gap-2 p-8">
        <span class="w-3 h-3 bg-[#ffaa33] rounded-full animate-bounce [animation-delay:0s]"></span>
        <span class="w-3 h-3 bg-[#ffaa33] rounded-full animate-bounce [animation-delay:0.2s]"></span>
        <span class="w-3 h-3 bg-[#ffaa33] rounded-full animate-bounce [animation-delay:0.4s]"></span>
      </div>

      <!-- Table wrapper -->
      <div v-else class="overflow-x-auto">
        <table class="w-full text-sm text-center table-auto">
          <thead>
            <tr class="bg-[#ffaa33] text-black">
              <th
                class="px-4 py-3"
                :class="filteredData.length === 0 ? 'rounded-bl-[16px]' : ''"
              >
                Id
              </th>
              <th class="px-4 py-3">{{ $t('Status') }}</th>
              <th class="px-4 py-3">{{ $t('Quantity') }}</th>
              <th class="px-4 py-3">{{ $t('Action Type') }}</th>
              <th class="px-4 py-3 hidden sm:table-cell">{{ $t('Reason') }}</th>
              <th class="px-4 py-3">{{ $t('Product') }}</th>
              <th class="px-4 py-3 hidden sm:table-cell">{{ $t('StorageRoom') }}</th>
              <th
                class="px-4 py-3"
                :class="filteredData.length === 0 ? 'rounded-br-[16px]' : ''"
                colspan="2"
              >
                {{ $t('Actions') }}
              </th>
            </tr>
          </thead>

          <tbody>
            <tr
              v-for="(item, index) in filteredData"
              :key="item.id"
              class="even:bg-white/5 hover:bg-[#2a2a2a]"
            >
              <td
                class="px-4 py-3"
                :class="index === filteredData.length - 1 ? 'rounded-bl-[16px]' : ''"
              >
                {{ item.id }}
              </td>
              <td class="px-4 py-3">
                <span
                  :class="[ 'px-3 py-1 rounded-full font-bold text-sm inline-block capitalize', {
                    'bg-green-600 text-white': item.status === 'Accepted',
                    'bg-red-600 text-white': item.status === 'Declined',
                    'bg-[#ffaa33] text-black': item.status === 'Pending',
                  }]"
                >
                  {{ item.status }}
                </span>
              </td>
              <td class="px-4 py-3">{{ item.quantity }}</td>
              <td class="px-4 py-3">{{ item.actionTypeName }}</td>
              <td class="px-4 py-3 hidden sm:table-cell">{{ item.reasonDescription }}</td>
              <td class="px-4 py-3">{{ item.productName }}</td>
              <td class="px-4 py-3 hidden sm:table-cell">{{ item.storageRoomName }}</td>
              <td class="px-2 py-3">
                <button
                  @click="editStatus(item.id, 'Accepted')"
                  class="bg-green-600 hover:scale-105 transition w-9 h-9 rounded-xl text-white flex items-center justify-center"
                >
                  <i class="bi bi-check-circle"></i>
                </button>
              </td>
              <td
                class="px-2 py-3"
                :class="index === filteredData.length - 1 ? 'rounded-br-[16px]' : ''"
              >
                <button
                  @click="editStatus(item.id, 'Declined')"
                  class="bg-red-600 hover:scale-105 transition w-9 h-9 rounded-xl text-white flex items-center justify-center"
                >
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

