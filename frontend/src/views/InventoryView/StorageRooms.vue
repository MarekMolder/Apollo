<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useSidebarStore } from '@/stores/sidebarStore'
import 'vue-multiselect/dist/vue-multiselect.min.css'
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService'
import { AddressService } from '@/services/mvcServices/AddressService'
import type { IStorageRoom } from '@/domain/logic/IStorageRoom'
import type { IStorageRoomEnriched } from '@/domain/logic/IStorageRoomEnriched'
import type { IAddress } from '@/domain/logic/IAddress'

// ---------------- Services ----------------
const storageRoomService = new StorageRoomService()
const addressService = new AddressService()

// ---------------- Entities ----------------
const data = ref<IStorageRoomEnriched[]>([])
const addresses = ref<IAddress[]>([])

// ---------------- Drawer Mode ----------------
const showHelp = ref(false)
const showDrawer = ref(false)
const drawerMode = ref<'edit' | 'create'>('create')
const activeEditRoom = ref<IStorageRoomEnriched | null>(null)
const activeCreateRoom = ref<IStorageRoom | null>(null)

// ---------------- Filters ----------------
const searchQuery = ref('')

// ---------------- Store && Router ----------------
const sidebarStore = useSidebarStore()
const router = useRouter()

// ---------------- Messages errors/success ----------------
const validationError = ref('')
const successMessage = ref('')

// ---------------- Empty Product entity ----------------
const emptyRoom = ref<IStorageRoom>({
  id: '',
  name: '',
  addressId: '',
  allowedRoles: []
})

// ---------------- Fetch ----------------
onMounted(async () => {
  await Promise.all([fetchPageData(), fetchAddresses()])
})

const fetchPageData = async () => {
  try {
    const result = await storageRoomService.getEnrichedStorageRooms()
    data.value = result.data || []
  } catch (error) {
    console.error('Error fetching storageRooms:', error)
  }
}

const fetchAddresses = async () => {
  try {
    const result = await addressService.getAllAsync()
    addresses.value = result.data || []
  } catch (e) {
    console.error('Error fetching addresses', e)
  }
}

// ---------------- Navigation ----------------
const goToCurrentStock = (storageRoomId: string) => {
  router.push(`/monthlyStatistics/${storageRoomId}`)
}

// ---------------- Search engine filtered storageRooms ----------------
const filteredStorageRooms = computed(() =>
  data.value.filter((storageRoom) =>
    storageRoom.name.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
)

const gridCols = computed(() => {
  const n = filteredStorageRooms.value.length
  if (n <= 1) return 'grid-cols-1'
  if (n === 2) return 'grid-cols-2'
  return 'grid-cols-3'
})

// ---------------- Drawers for storageRooms ----------------
const openCreateDrawer = () => {
  activeCreateRoom.value = { ...emptyRoom.value }
  drawerMode.value = 'create'
  showDrawer.value = true
  validationError.value = ''
  successMessage.value = ''
}

const openEditDrawer = (room: IStorageRoomEnriched) => {
  const { id, name, addressId, allowedRoles } = room
  activeEditRoom.value = { id, name, addressId, allowedRoles, fullAddress: room.fullAddress } as IStorageRoomEnriched
  drawerMode.value = 'edit'
  showDrawer.value = true
  validationError.value = ''
  successMessage.value = ''
}

const activeRoom = computed<Partial<IStorageRoom>>({
  get() {
    return drawerMode.value === 'edit' ? (activeEditRoom.value as IStorageRoom) : (activeCreateRoom.value as IStorageRoom)
  },
  set(v) {
    if (drawerMode.value === 'edit') {
      activeEditRoom.value = { ...(activeEditRoom.value as IStorageRoomEnriched), ...(v as IStorageRoom) }
    } else {
      activeCreateRoom.value = { ...(activeCreateRoom.value as IStorageRoom), ...(v as IStorageRoom) }
    }
  }
})

// ---------------- StorageRoom edit function ----------------
const updateRoom = async () => {
  validationError.value = ''
  successMessage.value = ''
  try {
    if (!activeEditRoom.value) return
    const payload: IStorageRoom = {
      id: activeEditRoom.value.id,
      name: activeEditRoom.value.name,
      addressId: activeEditRoom.value.addressId,
      allowedRoles: activeEditRoom.value.allowedRoles || []
    }
    const res = await storageRoomService.updateAsync(payload)
    if (res.errors?.length) {
      validationError.value = res.errors.join(', ')
      return
    }
    successMessage.value = '✅ Storage room updated!'
    showDrawer.value = false
    await fetchPageData()
  } catch (e) {
    console.error('Update room failed', e)
    validationError.value = 'Update failed. Check console.'
  }
}

// ---------------- StorageRoom create function ----------------
const createRoom = async () => {
  validationError.value = ''
  successMessage.value = ''
  try {
    if (!activeCreateRoom.value) return

    if (!activeCreateRoom.value.name?.trim()) {
      validationError.value = 'Name is required.'
      return
    }
    if (!activeCreateRoom.value.addressId) {
      validationError.value = 'Address is required.'
      return
    }

    const {id, ...payload} = activeCreateRoom.value
    const cleaned: IStorageRoom = {
      ...payload,
      allowedRoles: payload.allowedRoles?.filter(r => r && r.trim().length > 0) || []
    } as IStorageRoom

    const res = await storageRoomService.addAsync(cleaned)
    if (res.errors?.length) {
      validationError.value = res.errors.join(', ')
      return
    }

    successMessage.value = '✅ Storage room created!'
    showDrawer.value = false
    await fetchPageData()
  } catch (e) {
    console.error('Create room failed', e)
    validationError.value = 'Create failed. Check console.'
  }
}

const rolesInput = computed({
  get() {
    const arr = (drawerMode.value === 'edit' ? activeEditRoom.value?.allowedRoles : activeCreateRoom.value?.allowedRoles) || []
    return arr.join(',')
  },
  set(v: string) {
    const arr = v
      .split(',')
      .map(s => s.trim())
      .filter(Boolean)
    if (drawerMode.value === 'edit' && activeEditRoom.value) activeEditRoom.value.allowedRoles = arr
    if (drawerMode.value === 'create' && activeCreateRoom.value) activeCreateRoom.value.allowedRoles = arr
  }
})

// ---------------- StorageRoom remove function ----------------
const removeRoom = async (id: string) => {
  if (!confirm('Are you sure you want to delete this storage room?')) return
  try {
    await storageRoomService.removeAsync(id)
    await fetchPageData()
  } catch (e) {
    console.error('Delete room failed', e)
    alert('Failed to delete storage room.')
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
    <!-- HEADER -->
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

        <!-- Toolbar: search + create button -->
        <div class="mb-6 flex items-center justify-between gap-3 flex-wrap">
          <div class="flex items-center gap-2 min-w-[260px] flex-1">
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

          <button
            @click="openCreateDrawer"
            class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                   border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                   shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                   hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                   focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
            <i class="bi bi-plus-lg opacity-90 group-hover:opacity-100"></i>
            <span>New Storage Room</span>
          </button>
        </div>

        <!-- Grid of storage rooms -->
        <div class="grid gap-6" :class="gridCols">
          <div
            v-for="room in filteredStorageRooms"
            :key="room.id"
            class="relative rounded-xl p-5 bg-neutral-900/60 border border-neutral-700
                   shadow-[0_4px_12px_rgba(0,0,0,0.3)]
                   hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15 transition"
          >
            <!-- Remove -->
            <button
              class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                     border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                     hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
              @click="removeRoom(room.id)"
              title="Remove storage room">
              <i class="bi bi-trash3"></i>
            </button>

            <h2 class="text-2xl font-bold text-neutral-200 pr-10">{{ room.name }}</h2>
            <p class="text-base text-neutral-400 mt-2">📍 {{ room.fullAddress }}</p>

            <div class="mt-6 flex justify-end gap-2">
              <button
                @click="openEditDrawer(room)"
                class="inline-flex items-center gap-2 rounded-lg px-3.5 py-2 text-sm font-medium
                       border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                       shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_3px_10px_rgba(0,0,0,0.35)]
                       hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
                <i class="bi bi-pencil"></i>
                Edit
              </button>
              <button
                @click="goToCurrentStock(room.id)"
                class="inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                       border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent text-cyan-200
                       shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_4px_12px_rgba(0,0,0,0.35)]
                       hover:from-cyan-400/30 hover:via-cyan-300/20 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-cyan-400/40 transition no-underline"
              >
                <i class="bi bi-box-seam text-base"></i>
                Write-off statistics
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

    <!-- Create/Edit StorageRoom -->
    <transition name="fade">
      <div v-if="showDrawer" class="fixed inset-0 z-[1300] flex items-center justify-center bg-black/60 p-4" @click.self="showDrawer = false">
        <div
          class="w-full max-w-2xl rounded-2xl border border-neutral-700 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
                 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ drawerMode === 'edit' ? (activeEditRoom?.name || 'Edit Storage Room') : 'Create New Storage Room' }}
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                     border-1 border-neutral-700 bg-white/5 text-neutral-300
                     hover:bg-white/10 hover:text-white focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="showDrawer = false" title="Close">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <!-- Form -->
          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <!-- Name -->
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
              <input v-model="(activeRoom as any).name" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>

            <!-- Address -->
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Address</label>
              <select v-model="(activeRoom as any).addressId"
                      class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                             px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                             focus:border-neutral-500 transition shadow-inner shadow-black/30">
                <option disabled value="">Select address</option>
                <option v-for="a in addresses" :key="a.id" :value="a.id">{{ a.name }}</option>
              </select>
            </div>

            <!-- Allowed roles (as comma list OR switch to tag multiselect if you prefer) -->
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Allowed Roles (comma-separated)</label>
              <input v-model="rolesInput" type="text"
                     placeholder="e.g. admin,mustamäe"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
              <p class="mt-2 text-xs text-neutral-400">Tip: leave empty to allow no roles (or handle at backend as needed).</p>
            </div>
          </div>

          <!-- Messages -->
          <div class="mt-4">
            <p v-if="validationError" class="text-rose-400 text-center font-medium">{{ validationError }}</p>
            <p v-if="successMessage" class="text-emerald-400 text-center font-medium">{{ successMessage }}</p>
          </div>

          <!-- Actions -->
          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button
              v-if="drawerMode === 'edit'"
              @click="updateRoom"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Update
            </button>
            <button
              v-else
              @click="createRoom"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Create
            </button>
            <button
              @click="showDrawer = false"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </transition>

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
              See leht võimaldab <strong>otsida</strong>, <strong>lisada</strong>, <strong>muuta</strong> ja
              <strong>kustutada</strong> laoruume. Iga laoruumi kaardi pealt pääsed ka nupu <em>Write-off statistics</em> kaudu kuu statistika vaatesse.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Otsing:</strong> <em>Search storage room</em> filtreerib kaarte laoruumi nime järgi.
              </li>
              <li>
                <strong>Uus laoruum:</strong> vajuta <em>New Storage Room</em>, täida vorm ja salvesta.
              </li>
              <li>
                <strong>Muutmine:</strong> laoruumi kaardi nupp <em>Edit</em> avab valitud laoruumi andmete muutmise vormi.
              </li>
              <li>
                <strong>Kustutamine:</strong> prügikasti ikoon eemaldab laoruumi pärast kinnitust. Seda toimingut ei saa tagasi võtta.
              </li>
              <li>
                <strong>Address:</strong> vali olemasolev aadress rippmenüüst. (Aadresse saad hallata Settings &rarr; Addresses vaates.)
              </li>
              <li>
                <strong>Allowed Roles:</strong> sisesta rollid komadega eraldatult (nt <code>admin,mustamäe</code>).
                Tühjaks jätmisel ei lisata ühtegi rolli; Kasutajad, kellel on sama roll, mis laoruumil saavad teha selles laoruumis maha kandmisi.
              </li>
              <li>
                <strong>Write-off statistics:</strong> avab valitud laoruumi mahakandmiste kuuülevaate.
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaali saad sulgeda taustale klõpsates või ülanurga <em>×</em> nupust. Enne uute kirjete lisamist kasuta otsingut,
              et vältida duplikaate.
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
/* vue-multiselect (if you later switch roles input to tagging) */
:deep(.multiselect-dark) {
  @apply w-full rounded-xl border border-white/10 bg-neutral-900/70 text-white shadow-sm transition;
}
:deep(.multiselect-dark .multiselect__tags) { @apply flex items-center min-h-[44px] rounded-xl border-0 bg-transparent px-3 py-0; }
:deep(.multiselect-dark .multiselect__placeholder),
:deep(.multiselect-dark .multiselect__single) { @apply block p-0 m-0 bg-transparent text-neutral-300 leading-[44px]; }
:deep(.multiselect-dark .multiselect__input) { @apply bg-transparent text-white placeholder-neutral-500 leading-[44px] p-0 m-0; }
:deep(.multiselect-dark .multiselect__select),
:deep(.multiselect-dark .multiselect__clear) { @apply text-neutral-400 hover:text-white; }
:deep(.multiselect-dark.multiselect--active .multiselect__tags) { @apply ring-2 ring-[#ffaa33]/35 outline-none border-[#ffaa33]; }
:deep(.multiselect-dark .multiselect__content-wrapper) { @apply mt-2 rounded-xl border border-white/10 bg-neutral-950/95 backdrop-blur supports-[backdrop-filter]:bg-neutral-950/80 shadow-2xl max-h-64; }
:deep(.multiselect-dark .multiselect__content) { @apply py-1; }
:deep(.multiselect-dark .multiselect__option) { @apply px-4 py-2 text-neutral-200 cursor-pointer transition; }
:deep(.multiselect-dark .multiselect__option--highlight) { @apply bg-white/10; }
:deep(.multiselect-dark .multiselect__option--selected) { @apply bg-white/[0.06] text-[#ffaa33]; }
:deep(.multiselect-dark.multiselect--disabled) { @apply opacity-60 cursor-not-allowed; }
</style>
