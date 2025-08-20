<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';
import { RouterLink } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()
const showHelp = ref(false);

// ---------------- Services ----------------
const identityService = new IdentityService();

// ---------------- Fields ----------------
const firstName = ref('');
const lastName = ref('');
const userName = ref('');
const message = ref('');

// ---------------- Store ----------------
const store = useUserDataStore();

// ---------------- Fetch ----------------
onMounted(() => {
  firstName.value = store.firstName || '';
  lastName.value = store.lastName || '';
  userName.value = store.username || '';
});

// ---------------- Account edit function ----------------
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

    <!-- Card container -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div class="grid gap-5 md:grid-cols-2">
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
              Sellel lehel saad oma <strong>konto andmeid vaadata</strong> ja <strong>profiili uuendada</strong>.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Profiili muutmine:</strong> täida väljad <em>Firstname</em>, <em>Lastname</em> ja <em>Username</em> ning vajuta <em>Save data</em>.
              </li>
              <li>
                <strong>Parooli vahetamine:</strong> klõpsa nupul <em>Change password</em>, et liikuda parooli muutmise vaatesse.
              </li>
              <li>
                <strong>Teated:</strong> eduka salvestuse korral kuvatakse roheline kinnitus, vea korral punane teade koos põhjusega.
              </li>
            </ul>

            <div class="space-y-1">
              <p class="font-medium text-neutral-200">Nõuanded</p>
              <ul class="list-disc pl-6 space-y-1 text-neutral-300">
                <li>Väljad on eeltäidetud sinu praeguste andmetega; tühjaks jätmine ei muuda vastavat väärtust.</li>
                <li>Kasutajanime muutmine võib mõjutada sisselogimist — veendu, et mäletad uut kasutajanime.</li>
                <li>E-posti aadress on ainult vaadatav; kui vajad selle muutmist, pöördu administraatori poole (või kasuta vastavat vaadet, kui see on olemas).</li>
              </ul>
            </div>

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
