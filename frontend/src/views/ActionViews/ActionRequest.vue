<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import type { IResultObject } from '@/types/IResultObject'
import type { IActionEnriched } from '@/domain/logic/IActionEnriched.ts'
import { ActionService } from '@/services/mvcServices/ActionService.ts'
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();
const showHelp = ref(false);

// Services
const service = new ActionService()

// Entity's
const data = reactive<IResultObject<IActionEnriched[]>>({ data: [], errors: [] })

// ??
const requestIsOngoing = ref(false)

// Search engine
const selectedStatus = ref<'All' | 'Accepted' | 'Declined' | 'Pending'>('All')
const selectedUser = ref<'All' | string>('All')
const selectedMonth = ref<'All' | number>('All')
const selectedYear = ref<'All' | number>('All')

// Options (tuletame andmetest)
const userOptions = computed<string[]>(() => {
  const set = new Set<string>()
  for (const it of data.data ?? []) {
    if (it?.createdBy) set.add(it.createdBy)
  }
  return Array.from(set).sort((a, b) => a.localeCompare(b))
})

const yearOptions = computed<number[]>(() => {
  const set = new Set<number>()
  for (const it of data.data ?? []) {
    const d = it?.createdAt ? new Date(it.createdAt) : null
    if (d && !isNaN(d.getTime())) set.add(d.getFullYear())
  }
  return Array.from(set).sort((a, b) => b - a) // uuemad ees
})

const monthOptions = [
  { value: 1, label: 'Jan' },
  { value: 2, label: 'Feb' },
  { value: 3, label: 'Mar' },
  { value: 4, label: 'Apr' },
  { value: 5, label: 'May' },
  { value: 6, label: 'Jun' },
  { value: 7, label: 'Jul' },
  { value: 8, label: 'Aug' },
  { value: 9, label: 'Sep' },
  { value: 10, label: 'Oct' },
  { value: 11, label: 'Nov' },
  { value: 12, label: 'Dec' }
]

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
  let items = data.data ?? []

  if (selectedStatus.value !== 'All') {
    items = items.filter((i) => i.status === selectedStatus.value)
  }
  if (selectedUser.value !== 'All') {
    items = items.filter((i) => i.createdBy === selectedUser.value)
  }
  if (selectedYear.value !== 'All') {
    items = items.filter((i) => {
      const d = i?.createdAt ? new Date(i.createdAt) : null
      return d && d.getFullYear() === selectedYear.value
    })
  }
  if (selectedMonth.value !== 'All') {
    items = items.filter((i) => {
      const d = i?.createdAt ? new Date(i.createdAt) : null
      return d && d.getMonth() + 1 === selectedMonth.value
    })
  }

  return items
})

// Accept / Deckline function
const editStatus = async (id: string, newStatus: 'Accepted' | 'Declined', currentStatus: string) => {
  // UI-level kaitse (lõplikke ei muuda)
  if (currentStatus === 'Accepted' || currentStatus === 'Declined') return

  try {
    const response = await service.patchStatus(id, newStatus)
    console.log(response.message)
    await fetchPageData()
  } catch (error) {
    console.error('Status update failed', error)
  }
}

const acceptAll = async () => {
  const pending = filteredData.value.filter(i => i.status === 'Pending')
  for (const item of pending) {
    try {
      await editStatus(item.id, 'Accepted', item.status)
    } catch (e) {
      console.error('Failed to accept', item.id, e)
    }
  }
}
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
          Requests
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
    </section>

    <!-- Kaart -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8
               bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
               border-1 border-neutral-700
               shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]">

        <!-- Toolbar: filtrid -->
        <div class="mb-6 grid grid-cols-1 md:grid-cols-2 xl:grid-cols-5 gap-3 items-center">
          <!-- Staatus -->
          <div class="flex items-center gap-2">
            <i class="bi bi-funnel text-neutral-400 hidden sm:inline"></i>
            <label class="sr-only">Status</label>
            <div class="relative w-full">
              <select
                v-model="selectedStatus"
                class="peer w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
               px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-400/30
               focus:border-neutral-500 transition shadow-inner shadow-black/30">
                <option value="All">{{ $t('All') }}</option>
                <option value="Accepted">{{ $t('Accepted') }}</option>
                <option value="Declined">{{ $t('Declined') }}</option>
                <option value="Pending">{{ $t('Pending') }}</option>
              </select>
              <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>
          </div>

          <!-- Kasutaja -->
          <div class="relative">
            <label class="sr-only">User</label>
            <select
              v-model="selectedUser"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
             px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-400/30
             focus:border-neutral-500 transition shadow-inner shadow-black/30">
              <option value="All">{{ $t('All users') }}</option>
              <option v-for="u in userOptions" :key="u" :value="u">{{ u }}</option>
            </select>
            <i class="bi bi-person absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Kuu -->
          <div class="relative">
            <label class="sr-only">Month</label>
            <select
              v-model="selectedMonth"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
             px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-400/30
             focus:border-neutral-500 transition shadow-inner shadow-black/30">
              <option value="All">{{ $t('All months') }}</option>
              <option v-for="m in monthOptions" :key="m.value" :value="m.value">{{ m.label }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Aasta -->
          <div class="relative">
            <label class="sr-only">Year</label>
            <select
              v-model="selectedYear"
              class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
             px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-cyan-400/30
             focus:border-neutral-500 transition shadow-inner shadow-black/30">
              <option value="All">{{ $t('All years') }}</option>
              <option v-for="y in yearOptions" :key="y" :value="y">{{ y }}</option>
            </select>
            <i class="bi bi-calendar-event absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <!-- Nupp -->
          <div class="flex xl:justify-end">
            <router-link
              to="/createaction"
              class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2 text-sm font-semibold
             border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
             shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_8px_24px_rgba(0,0,0,0.35)]
             hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
             focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition
             w-full xl:w-auto no-underline">
              <i class="bi bi-trash3-fill opacity-80 group-hover:opacity-100"></i>
              <span>{{ $t('Dispose item') }}</span>
            </router-link>

            <button
              @click="acceptAll"
              class="ml-2 w-14 h-12 rounded-xl flex flex-col items-center justify-center
         ring-1 ring-emerald-400/30 bg-emerald-500/10 text-emerald-300
         hover:bg-emerald-500/15 hover:scale-[1.05] transition"
            >
              <!-- Tekst üleval -->
              <span class="text-[10px] font-medium leading-tight">Accept</span>

              <!-- Ikoon all -->
              <i class="bi bi-check2-all text-lg"></i>
            </button>

          </div>
        </div>

        <!-- Loading -->
        <div v-if="!requestIsOngoing" class="flex justify-center gap-2 p-12">
          <span class="w-3 h-3 bg-neutral-400 rounded-full animate-bounce [animation-delay:0s]"></span>
          <span class="w-3 h-3 bg-neutral-400 rounded-full animate-bounce [animation-delay:0.2s]"></span>
          <span class="w-3 h-3 bg-neutral-400 rounded-full animate-bounce [animation-delay:0.4s]"></span>
        </div>

        <!-- Tabel -->
        <div v-else class="overflow-x-auto rounded-[12px] border border-neutral-700">
          <table class="w-full text-sm text-left table-fixed">
            <!-- määrame samad veerulaiused thead/tbody jaoks -->
            <colgroup>
              <col class="w-[10rem]" />   <!-- Product -->
              <col class="w-[12rem]" />   <!-- Reason -->
              <col class="w-[8rem]" />    <!-- Quantity -->
              <col class="w-[12rem]" />   <!-- Requested by -->
              <col class="w-[12rem]" />   <!-- Requested at -->
              <col class="w-[12rem]" />   <!-- StorageRoom -->
              <col class="w-[9rem]" />    <!-- Status -->
              <col class="w-[3.5rem]" />  <!-- Accept -->
              <col class="w-[3.5rem]" />  <!-- Decline -->
            </colgroup>

            <thead class="bg-neutral-900/70 text-neutral-300">
            <tr>
              <th class="px-4 py-3 font-semibold">{{ $t('Product') }}</th>
              <th class="px-4 py-3 font-semibold hidden sm:table-cell">{{ $t('Reason') }}</th>
              <th class="px-4 py-3 font-semibold">{{ $t('Quantity') }}</th>
              <th class="px-4 py-3 font-semibold hidden lg:table-cell">{{ $t('Requested by') }}</th>
              <th class="px-4 py-3 font-semibold hidden lg:table-cell">{{ $t('Requested at') }}</th>
              <th class="px-4 py-3 font-semibold hidden sm:table-cell">{{ $t('StorageRoom') }}</th>
              <th class="px-4 py-3 font-semibold">{{ $t('Status') }}</th>
              <th class="px-4 py-3 font-semibold text-center" colspan="2">{{ $t('Actions') }}</th>
            </tr>
            </thead>

            <tbody>
            <tr
              v-for="item in filteredData"
              :key="item.id"
              class="border-t border-white/10 even:bg-white/5 hover:bg-white/10 transition">
              <td class="px-4 py-3 align-middle">{{ item.productName }}</td>
              <td class="px-4 py-3 align-middle hidden sm:table-cell">{{ item.reasonDescription }}</td>
              <td class="px-4 py-3 align-middle">{{ item.quantity }} {{ item.productUnit }}</td>
              <td class="px-4 py-3 align-middle hidden lg:table-cell">{{ item.createdBy }}</td>
              <td class="px-4 py-3 align-middle hidden lg:table-cell whitespace-nowrap">
                <time :datetime="item.createdAt">{{ new Date(item.createdAt).toLocaleString() }}</time>
              </td>
              <td class="px-4 py-3 align-middle hidden sm:table-cell">{{ item.storageRoomName }}</td>

              <td class="px-4 py-3 align-middle">
          <span
            class="px-3 py-1 rounded-full font-medium text-xs inline-block capitalize ring-1"
            :class="{
              'bg-emerald-500/10 text-emerald-300 ring-emerald-400/30': item.status === 'Accepted',
              'bg-rose-500/10 text-rose-300 ring-rose-400/30': item.status === 'Declined',
              'bg-neutral-300/10 text-neutral-300 ring-neutral-400/30': item.status === 'Pending'
            }">
            {{ item.status }}
          </span>
              </td>

              <td class="px-2 py-3 align-middle text-center">
                <button
                  :disabled="item.status !== 'Pending'"
                  @click="editStatus(item.id, 'Accepted', item.status)"
                  class="w-9 h-9 rounded-xl inline-flex items-center justify-center
                   ring-1 ring-emerald-400/30 bg-emerald-500/10 text-emerald-300
                   hover:bg-emerald-500/15 hover:scale-[1.03] transition
                   disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:scale-100">
                  <i class="bi bi-check-circle"></i>
                </button>
              </td>
              <td class="px-2 py-3 align-middle text-center">
                <button
                  :disabled="item.status !== 'Pending'"
                  @click="editStatus(item.id, 'Declined', item.status)"
                  class="w-9 h-9 rounded-xl inline-flex items-center justify-center
                   ring-1 ring-rose-400/30 bg-rose-500/10 text-rose-300
                   hover:bg-rose-500/15 hover:scale-[1.03] transition
                   disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:scale-100">
                  <i class="bi bi-x-circle"></i>
                </button>
              </td>
            </tr>

            <tr v-if="filteredData.length === 0">
              <td colspan="9" class="px-4 py-10 text-center text-neutral-400">
                {{ $t('No data to display') }}
              </td>
            </tr>
            </tbody>
          </table>
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
              See vaade võimaldab <strong>kinnitada</strong> või <strong>tagasi lükata</strong> kasutajate sisestatud mahakandmise taotlusi
              ning filtreerida neid staatuse, kasutaja ja aja järgi.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Filtrid:</strong> ülariba <em>Status / User / Month / Year</em> piiravad alloleva tabeli kirjeid.
                <em>All</em> tähendab, et filtrit ei rakendata.
              </li>
              <li>
                <strong>Tabeli veerud:</strong> toode, põhjus, kogus, taotleja, taotluse aeg, laoruum, staatus ja toimingud.
                Aeg kuvatakse lokaalses formaadis.
              </li>
              <li>
                <strong>Staatused:</strong> <em>Pending</em> (ootel), <em>Accepted</em> (kinnitatud), <em>Declined</em> (tagasi lükatud).
                Kui kirje pole enam <em>Pending</em>, ei saa seda muuta.
              </li>
              <li>
                <strong>Accept / Decline:</strong> rea lõpus olevate nuppudega kinnitad või lükkad tagasi üksiku taotluse.
              </li>
              <li>
                <strong>Accept (kõik nähtavad):</strong> tööriistariba roheline nupp kinnitab <em>kõik hetkel filtritega nähtavad</em>
                <em>Pending</em> taotlused.
              </li>
              <li>
                <strong>Dispose item:</strong> viib uuele lehele, kus saab lisada uue mahakandmise taotluse.
              </li>
              <li>
                <strong>Laadimine:</strong> kui andmeid kogutakse, kuvatakse lühidalt laadimisindikaator.
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: kui soovid kinnitada vaid kindla kuu/kasutaja taotlused, rakenda vastavad filtrid enne
              <em>Accept (kõik nähtavad)</em> nupu vajutamist. Modaali saab sulgeda taustale klõpsates või ülanurga sulgemisnupust.
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


