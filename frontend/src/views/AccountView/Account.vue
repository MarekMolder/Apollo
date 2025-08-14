<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';
import { RouterLink } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()

// Services
const identityService = new IdentityService();

//??
const firstName = ref('');
const lastName = ref('');
const userName = ref('');
const message = ref('');

// Store
const store = useUserDataStore();

// Get account info
onMounted(() => {
  firstName.value = store.firstName || '';
  lastName.value = store.lastName || '';
  userName.value = store.username || '';
});

// Account edit function
const editAccount = async () => {
  const result = await identityService.updateUserFields({
    email: store.email!,
    userName: userName.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });

  message.value = result.errors ? `Viga: ${result.errors.join(', ')}` : '✅ Konto uuendatud!';
};
</script>

<template>
  <main
    :class="[
      // ühtne paigutus kõigil lehtedel
      'transition-all duration-300 p-6 sm:p-8 text-white font-[Inter,sans-serif] bg-transparent max-w-screen-2xl',
      sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]'
    ]"
  >
    <!-- Header -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)] relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          {{ $t('My account') }}
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">{{ $t('View and update your profile') }}</p>
    </section>

    <!-- Kaardikonteiner samas stiilis -->
    <section class="mx-auto w-full max-w-[100rem]">
        <!-- kaks kaarti kõrvuti, virna väiksel -->
      <div class="grid gap-5 md:grid-cols-2">
          <!-- Info kaart -->
          <div class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-4 sm:p-5">
            <div class="flex items-center gap-4 mb-5">
              <div
                class="w-[72px] h-[72px] rounded-full flex items-center justify-center text-3xl
                       bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent
                       ring-1 ring-cyan-400/30 text-cyan-200">
                👤
              </div>
              <h2 class="text-xl font-semibold text-neutral-100">{{ $t('Account info') }}</h2>
            </div>

            <div class="divide-y divide-white/10 text-sm">
              <div class="py-3 flex items-center justify-between">
                <span class="text-neutral-400">{{ $t('Firstname') }}</span>
                <span class="text-neutral-100 font-medium">{{ store.firstName }}</span>
              </div>
              <div class="py-3 flex items-center justify-between">
                <span class="text-neutral-400">{{ $t('Lastname') }}</span>
                <span class="text-neutral-100 font-medium">{{ store.lastName }}</span>
              </div>
              <div class="py-3 flex items-center justify-between">
                <span class="text-neutral-400">{{ $t('Username') }}</span>
                <span class="text-neutral-100 font-medium">{{ store.username }}</span>
              </div>
              <div class="py-3 flex items-center justify-between">
                <span class="text-neutral-400">{{ $t('Email') }}</span>
                <span class="text-neutral-100 font-medium">{{ store.email }}</span>
              </div>
            </div>
          </div>

          <!-- Muuda profiili kaart -->
        <div class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-4 sm:p-5">
          <h2 class="text-lg font-semibold text-neutral-100 mb-5">{{ $t('Edit Profile') }}</h2>

            <form @submit.prevent="editAccount" class="grid grid-cols-1 gap-4">
              <div>
                <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">{{ $t('Firstname') }}</label>
                <input
                  v-model="firstName"
                  type="text"
                  class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                         placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
                  :placeholder="$t('Firstname') as string"
                />
              </div>

              <div>
                <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">{{ $t('Lastname') }}</label>
                <input
                  v-model="lastName"
                  type="text"
                  class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                         placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
                  :placeholder="$t('Lastname') as string"
                />
              </div>

              <div>
                <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">{{ $t('Username') }}</label>
                <input
                  v-model="userName"
                  type="text"
                  class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                         placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
                  :placeholder="$t('Username') as string"
                />
              </div>

              <div class="mt-2 flex flex-col sm:flex-row gap-3 sm:justify-between">
                <button
                  type="submit"
                  class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                         border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                         shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                         hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                         focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition w-full sm:w-auto"
                >
                  {{ $t('Save data') }}
                </button>

                <RouterLink
                  to="/changepassword"
                  class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                         border-1 border-neutral-700 bg-white/5 text-neutral-200 no-underline
                         hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition w-full sm:w-auto text-center"
                >
                  {{ $t('Change password') }}
                </RouterLink>
              </div>

              <p class="text-center font-medium text-emerald-400 mt-2">{{ message }}</p>
            </form>
          </div>
        </div>
    </section>
  </main>
</template>
