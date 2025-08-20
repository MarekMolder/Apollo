<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { IdentityService } from '@/services/IdentityService';
import { useUserDataStore } from '@/stores/userDataStore';
import { useSidebarStore } from '@/stores/sidebarStore'

// ---------------- Services ----------------
const identityService = new IdentityService();

// ---------------- Store, router and empty strings ----------------
const router = useRouter();
const store = useUserDataStore();
const sidebarStore = useSidebarStore()
const email = ref('');
const password = ref('');
const firstName = ref('');
const lastName = ref('');
const showHelp = ref(false);

// ---------------- Messages errors/success ----------------
const validationError = ref<string | null>(null);
const successMessage = ref<string | null>(null);

// ---------------- isAdmin ----------------
const isAdmin = Array.isArray(store.roles)
  ? store.roles.some(r => r === 'admin' || r === 'manager')
  : store.roles === 'admin' || store.roles === 'manager';

// ---------------- Register function ----------------
const doRegister = async () => {
  validationError.value = null;
  successMessage.value = null;

  const result = await identityService.register({
    email: email.value,
    password: password.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });

  if (result?.data) {
    if (!isAdmin) {
      store.jwt = result.data.jwt;
      store.refreshToken = result.data.refreshToken;
      router.push({ name: 'Home' });
    } else {
      successMessage.value = 'Account successfully created';
      email.value = '';
      password.value = '';
      firstName.value = '';
      lastName.value = '';
    }
  } else {
    validationError.value = result.errors?.[0] || 'Registration failed';
  }
};
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-6 sm:p-8 text-white font-[Inter,sans-serif] bg-transparent max-w-screen-2xl flex justify-center',
      sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]'
    ]"
  >
    <!-- HEADER -->
    <section class="w-full max-w-[32rem]">
      <section class="mb-8 text-center">
        <h1
          class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
                 drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)] relative inline-block"
        >
          <span
            class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent"
          >
            {{ $t('Register') }}
          </span>
        </h1>
        <div
          class="mt-4 mx-auto h-px w-128 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"
        ></div>
        <p class="mt-3 text-sm text-neutral-400">
          {{ $t('Create a new account') }}
        </p>
      </section>

      <!-- Card container -->
      <div
        class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-5 sm:p-6 shadow-[0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl"
      >
        <div
          v-if="validationError"
          class="mb-4 text-center bg-[rgba(255,80,80,0.2)] text-[#ff6b6b] border border-[rgba(255,80,80,0.4)] font-medium text-sm p-3 rounded-lg"
        >
          {{ validationError }}
        </div>

        <div
          v-if="successMessage"
          class="mb-4 text-center bg-[rgba(80,255,160,0.2)] text-[#80ffaa] border border-[rgba(80,255,160,0.4)] font-medium text-sm p-3 rounded-lg"
        >
          {{ successMessage }}
        </div>

        <form @submit.prevent="doRegister" class="grid grid-cols-1 gap-4">
          <div>
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
              {{ $t('Firstname') }}
            </label>
            <input
              v-model="firstName"
              type="text"
              required
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                     placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              :placeholder="$t('Firstname') as string"
            />
          </div>

          <div>
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
              {{ $t('Lastname') }}
            </label>
            <input
              v-model="lastName"
              type="text"
              required
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                     placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              :placeholder="$t('Lastname') as string"
            />
          </div>

          <div>
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
              {{ $t('Email') }}
            </label>
            <input
              v-model="email"
              type="email"
              required
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                     placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              :placeholder="$t('Email') as string"
            />
          </div>

          <div>
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
              {{ $t('Password') }}
            </label>
            <input
              v-model="password"
              type="password"
              required
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                     placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              :placeholder="$t('Password') as string"
            />
          </div>

          <button
            type="submit"
            class="mt-2 inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                   border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                   shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                   hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                   focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition w-full"
          >
            {{ $t('Register') }}
          </button>
        </form>
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
              Sellel lehel saad luua <strong>uue kasutajakonto</strong>. Täida vorm ning kinnita nupuga
              <em>Register</em>.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Vormiväljad:</strong> <em>Firstname</em>, <em>Lastname</em>, <em>Email</em> ja
                <em>Password</em> on kohustuslikud. Kasuta kehtivat e-posti aadressi ja tugevat parooli.
              </li>
              <li>
                <strong>Tulemus pärast registreerimist:</strong>
                <ul class="list-disc pl-6 mt-1 space-y-1">
                  <li><em>Admin/Manager:</em> konto luuakse, vorm tühjendatakse ning kuvatakse kinnitus
                    “Account successfully created”.</li>
                </ul>
              </li>
              <li>
                <strong>Veateated:</strong> kui midagi läheb valesti (nt e-post on juba kasutusel),
                ilmub punane veateade vormi kohal.
              </li>
              <li>
                <strong>Turvalisus:</strong> parool sisestatakse peidetult. Ära jaga sisselogimisandmeid
                ja kasuta ainulaadset parooli.
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
