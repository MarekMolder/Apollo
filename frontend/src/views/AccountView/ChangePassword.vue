<script setup lang="ts">
import { useUserDataStore } from "@/stores/userDataStore.ts";
import { IdentityService } from "@/services/IdentityService.ts";
import { ref } from "vue";
import { RouterLink } from "vue-router";
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()
const showHelp = ref(false);

// Services
const identityService = new IdentityService();

// Store
const store = useUserDataStore();

// ??
const currentPassword = ref('');
const newPassword = ref('');
const confirmNewPassword = ref('');

// Messages errors/success
const message = ref('');

// Password edit function
const changePassword = async () => {
  if (newPassword.value !== confirmNewPassword.value) {
    message.value = 'New password and confirmation password arent same';
    return;
  }

  const result = await identityService.changePassword({
    email: store.email!,
    currentPassword: currentPassword.value,
    newPassword: newPassword.value,
  });

  message.value = result.errors
    ? `Error: ${result.errors.join(', ')}`
    : 'Password changed successfully!';
};
</script>

<template>
  <main
    :class="[
    sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]',
    'transition-all duration-300 px-6 sm:px-8 py-8',
    'flex items-start justify-center text-white font-[Inter,sans-serif] bg-transparent',
    'pt-16 sm:pt-20 min-h-screen'
  ]"
  >
    <!-- Keskosa wrapper, piiratud laius -->
    <div class="w-full max-w-lg">
      <!-- Header väljaspool kasti, keskel -->
      <section class="mb-6 text-center">
        <h1
          class="text-3xl sm:text-4xl font-[Playfair_Display] font-bold tracking-[0.02em]
                 drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]"
        >
          <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
            {{ $t('Change password') }}
          </span>
        </h1>
        <div class="mt-4 mx-auto h-px w-64 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
        <p class="mt-3 text-sm text-neutral-400">
          {{ $t('Update your password securely') }}
        </p>
      </section>

      <!-- Kaart -->
      <section>
        <div
          class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-5 sm:p-6
                 shadow-[0_0_0_1px_rgba(255,255,255,0.02),_0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl"
        >
          <form @submit.prevent="changePassword" class="grid grid-cols-1 gap-4">
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Enter current password') }}
              </label>
              <input
                v-model="currentPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Enter current password') as string"
              />
            </div>

            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Enter new password') }}
              </label>
              <input
                v-model="newPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Enter new password') as string"
              />
            </div>

            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">
                {{ $t('Confirm new password') }}
              </label>
              <input
                v-model="confirmNewPassword"
                type="password"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                       placeholder-neutral-500 outline-none transition focus:border-[#ffaa33]/50 focus:ring-2 focus:ring-[#ffaa33]/25"
                :placeholder="$t('Confirm new password') as string"
              />
            </div>

            <div class="mt-2 flex flex-col sm:flex-row gap-3 sm:justify-between">
              <button
                type="submit"
                class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                       border-1 border-neutral-700 bg-gradient-to-br from-[#ffaa33]/20 via-[#ffaa33]/10 to-transparent text-[#ffaa33]
                       shadow-[0_0_0_1px_rgba(255,170,51,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                       hover:from-[#ffaa33]/30 hover:via-[#ffaa33]/20 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-[#ffaa33]/30 transition w-full sm:w-auto"
              >
                {{ $t('Change password') }}
              </button>

              <RouterLink
                to="/account"
                class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                       border-1 border-neutral-700 bg-white/5 text-neutral-200 no-underline
                       hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition w-full sm:w-auto text-center"
              >
                {{ $t('Back') }}
              </RouterLink>
            </div>

            <p
              v-if="message"
              :class="message.includes('Error')
                ? 'mt-2 text-center font-medium px-4 py-2 rounded-md bg-red-500/10 border border-red-500/20 text-red-400'
                : 'mt-2 text-center font-medium px-4 py-2 rounded-md bg-emerald-500/10 border border-emerald-500/20 text-emerald-400'"
            >
              {{ message }}
            </p>
          </form>
        </div>
      </section>
    </div>

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
              Sellel lehel saad turvaliselt oma konto <strong>parooli vahetada</strong>. Täida vorm, kinnita uus parool ja salvesta muudatus.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Praegune parool:</strong> sisesta kehtiv parool väljale <em>Enter current password</em>.
              </li>
              <li>
                <strong>Uus parool:</strong> kirjuta soovitud uus parool väljale <em>Enter new password</em>.
              </li>
              <li>
                <strong>Kinnitus:</strong> korda uut parooli väljale <em>Confirm new password</em>.
                Kui väljad ei kattu, kuvatakse teade „New password and confirmation password aren't same”.
              </li>
              <li>
                <strong>Salvestamine:</strong> vajuta <em>Change password</em>.
                Õnnestumisel kuvatakse kinnitus „Password changed successfully!”.
                Kui backend tagastab vead (nt vale praegune parool või paroolipoliitika rikkumine), kuvatakse <em>Error:</em> teade.
              </li>
            </ul>

            <div class="space-y-1 text-neutral-300">
              <p class="font-medium">Hea parooli soovitused:</p>
              <ul class="list-disc pl-6 space-y-1 text-neutral-300">
                <li>vähemalt 8–12 märki, koos suur- ja väiketähtede, numbrite ja sümbolitega;</li>
                <li>väldi korduskasutust ja äratuntavaid mustreid (nt <code>Passw0rd!</code>);</li>
                <li>ära jaga parooli ja ära salvesta seda avalikult nähtavatesse kohtadesse.</li>
              </ul>
            </div>

            <p class="text-neutral-400 text-sm">
              Nipp: modaali saab sulgeda taustale klõpsates või ülanurga sulgemisnupust.
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

