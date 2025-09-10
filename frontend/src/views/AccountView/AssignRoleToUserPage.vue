<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { RoleService } from '@/services/RoleService'
import { IdentityService } from '@/services/IdentityService'
import type { AppRole } from '@/domain/logic/AppRole'
import type { AssignRoleDto } from '@/types/AssignRoleDto'
import { useSidebarStore } from '@/stores/sidebarStore'
import Multiselect from 'vue-multiselect'
import 'vue-multiselect/dist/vue-multiselect.min.css'

// ---------------- Services ----------------
const roleService = new RoleService()
const identityService = new IdentityService()

// ---------------- Entities ----------------
const roles = ref<AppRole[]>([])

// ---------------- Stores, drawers and users ----------------
const sidebarStore = useSidebarStore()
const showHelp = ref(false)

type UserLite = { id: string; firstName: string; lastName: string }
const users = ref<UserLite[]>([])

// --- Multiselect option & selection types ---
type SelectOpt = { label: string; value: string }
const selectedUser = ref<SelectOpt | null>(null)
const selectedRole = ref<SelectOpt | null>(null)

// ---------------- Messages errors/success ----------------
const successMessage = ref('')
const validationError = ref('')

// ---------------- Helpers ----------------
const isGuid = (s: string) =>
  /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(s)

// ---------------- Assign role to user function----------------
const assignRole = async () => {
  const userId = selectedUser.value?.value ?? ''
  const roleId = selectedRole.value?.value ?? ''

  if (!isGuid(userId) || !isGuid(roleId)) {
    validationError.value = 'Vali kasutaja ja roll (vigane või puudu GUID).'
    successMessage.value = ''
    return
  }

  const dto: AssignRoleDto = { userId, roleId }
  const res = await roleService.assignRoleToUser(dto)

  if (res.errors && res.errors.length) {
    // Tõsta sõnum nähtavaks ka siis, kui backend tagastab objekti
    validationError.value = typeof res.errors[0] === 'string'
      ? res.errors[0]
      : JSON.stringify(res.errors[0])
    successMessage.value = ''
  } else {
    successMessage.value = res.data ?? 'Role assigned to user'
    validationError.value = ''
    // Soovi korral nulli valikud:
    // selectedUser.value = null
    // selectedRole.value = null
  }
}

// ---------------- Fetch ----------------
onMounted(async () => {
  users.value = await identityService.getAllUsers()
  roles.value = await roleService.getAllRoles()
})

// ---------------- Options ----------------
const userOptions = computed<SelectOpt[]>(
  () => users.value.map(u => ({ label: `${u.firstName} ${u.lastName}`, value: u.id }))
)
const roleOptions = computed<SelectOpt[]>(
  () => roles.value.map(r => ({ label: r.name, value: r.id }))
)
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-6 sm:p-8 text-white font-[Inter,sans-serif] bg-transparent max-w-screen-md mx-auto',
      sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]'
    ]"
  >
    <!-- Header -->
    <section class="mb-8 text-center">
      <h1
        class="text-3xl sm:text-4xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]"
      >
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          {{ $t('Assign role to user') }}
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-64 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">
        {{ $t('Select a user and assign a role') }}
      </p>
    </section>

    <!-- Card -->
    <div
      class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-6 sm:p-8 shadow-[0_0_0_1px_rgba(255,255,255,0.02),_0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl"
    >
      <!-- User -->
      <div class="mb-4">
        <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
          {{ $t('Select user') }}
        </label>
        <Multiselect
          v-model="selectedUser"
          :options="userOptions"
          label="label"
          track-by="value"
          :searchable="true"
          :close-on-select="true"
          :allow-empty="false"
          placeholder="Search user..."
          class="multiselect-dark w-full"
        />
      </div>

      <!-- Role -->
      <div class="mb-6">
        <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
          {{ $t('Select role') }}
        </label>
        <Multiselect
          v-model="selectedRole"
          :options="roleOptions"
          label="label"
          track-by="value"
          :searchable="true"
          :close-on-select="true"
          :allow-empty="false"
          placeholder="Search role..."
          class="multiselect-dark w-full"
        />
      </div>

      <!-- Button -->
      <button
        class="w-full inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
               border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
               shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
               hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
               focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition
               disabled:opacity-50 disabled:cursor-not-allowed"
        :disabled="!selectedUser || !selectedRole"
        @click="assignRole"
      >
        {{ $t('Assign') }}
      </button>

      <!-- Messages -->
      <p
        v-if="successMessage"
        class="mt-4 text-center text-sm font-medium px-4 py-2 rounded-md bg-emerald-500/10 border border-emerald-500/20 text-emerald-400"
      >
        {{ successMessage }}
      </p>
      <p
        v-if="validationError"
        class="mt-4 text-center text-sm font-medium px-4 py-2 rounded-md bg-red-500/10 border border-red-500/20 text-red-400"
      >
        {{ validationError }}
      </p>
    </div>

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
          <!-- (sama sisu nagu enne) -->
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

          <div class="mt-5 space-y-4 text-neutral-200 leading-relaxed">
            <!-- ... sinu helpi tekst ... -->
            <p>
              Selle lehega saad <strong>määrata rolli</strong> valitud kasutajale. Vali allolevatest rippmenüüdest kasutaja ja roll ning kinnita.
            </p>
            <!-- jne -->
          </div>

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
      </div>
    </transition>
  </main>
</template>

<style scoped>
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
:deep(.multiselect-dark .multiselect__option) { @apply px-4 py-2 text-neutral-200 cursor-pointer transition; }
:deep(.multiselect-dark .multiselect__option--highlight) { @apply bg-white/10; }
:deep(.multiselect-dark .multiselect__option--selected) { @apply bg-white/[0.06] text-[#ffaa33]; }
:deep(.multiselect-dark.multiselect--disabled) { @apply opacity-60 cursor-not-allowed; }
</style>
