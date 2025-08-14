<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { RoleService } from '@/services/RoleService'
import { IdentityService } from '@/services/IdentityService'
import type { AppRole } from '@/domain/logic/AppRole'
import type { AssignRoleDto } from '@/types/AssignRoleDto'
import { useSidebarStore } from '@/stores/sidebarStore'
import Multiselect from "vue-multiselect";
import 'vue-multiselect/dist/vue-multiselect.min.css'
const sidebarStore = useSidebarStore()


// Services
const roleService = new RoleService()
const identityService = new IdentityService()

// Entity's
const roles = ref<AppRole[]>([])

// ??
const users = ref<{ id: string; firstName: string; lastName: string }[]>([])

// ?
const selectedUserId = ref('')
const selectedRoleId = ref('')

// Messages errors/success
const successMessage = ref('')
const validationError = ref('')

// Get users and roles
onMounted(async () => {
  users.value = await identityService.getAllUsers()
  roles.value = await roleService.getAllRoles()
})

// Assign Role to user function
const assignRole = async () => {
  const dto: AssignRoleDto = {
    userId: selectedUserId.value,
    roleId: selectedRoleId.value,
  }
  const res = await roleService.assignRoleToUser(dto)
  if (res.errors!.length) {
    validationError.value = res.errors![0]
    successMessage.value = ''
  } else {
    successMessage.value = res.data!
    validationError.value = ''
  }
}

// Tüübid + reducer
type SelectOpt = { label: string; value: string }
const reduceToValue = (opt: SelectOpt) => opt.value

// Ehitame valikud tüübitult
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
          v-model="selectedUserId"
          :options="userOptions"
          label="label"
          track-by="value"
          :reduce="reduceToValue"
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
          v-model="selectedRoleId"
          :options="roleOptions"
          label="label"
          track-by="value"
          :reduce="reduceToValue"
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
               focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition"
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
