<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { IdentityService } from '@/services/IdentityService'
import { useSidebarStore } from '@/stores/sidebarStore'

const sidebarStore = useSidebarStore()
const showHelp = ref(false)

const identityService = new IdentityService()
const route = useRoute()
const router = useRouter()

const loading = ref(true)
const error = ref<string | null>(null)

const user = ref<{
  id: string
  email: string
  firstName: string
  lastName: string
  userName: string
} | null>(null)

onMounted(async () => {
  try {
    const id = route.params.id as string
    user.value = await identityService.getUserById(id)
  } catch (e: any) {
    error.value = e?.message ?? 'Failed to load user'
  } finally {
    loading.value = false
  }
})

const goBack = () => router.back()
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-6 sm:p-8 text-white font-[Inter,sans-serif] bg-transparent max-w-screen-2xl',
      sidebarStore.isOpen ? 'ml-[165px]' : 'ml-[64px]'
    ]"
  >
    <!-- Header (sama stiil kui Account) -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)] relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          {{ $t('User Details') }}
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">{{ $t('View selected user profile') }}</p>
    </section>

    <!-- Kaardikonteiner (sama skeem) -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div class="grid gap-5 place-items-center">
        <!-- Info kaart -->
        <div
          class="w-full max-w-[640px] mx-auto rounded-xl border border-neutral-700 bg-neutral-900/60 p-4 sm:p-5"
        >          <div class="flex items-center gap-4 mb-5">
            <div
              class="w-[72px] h-[72px] rounded-full flex items-center justify-center text-3xl
                     bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent
                     ring-1 ring-cyan-400/30 text-cyan-200">
              👤
            </div>
            <h2 class="text-xl font-semibold text-neutral-100">{{ $t('Account info') }}</h2>
          </div>

          <div v-if="loading" class="py-6 text-center italic text-neutral-400">
            {{ $t('Loading...') }}
          </div>
          <p v-if="error" class="py-3 text-center text-rose-400">
            {{ error }}
          </p>

          <div v-if="user && !loading" class="divide-y divide-white/10 text-sm">
            <div class="py-3 flex items-center justify-between">
              <span class="text-neutral-400">{{ $t('Firstname') }}</span>
              <span class="text-neutral-100 font-medium">{{ user.firstName }}</span>
            </div>
            <div class="py-3 flex items-center justify-between">
              <span class="text-neutral-400">{{ $t('Lastname') }}</span>
              <span class="text-neutral-100 font-medium">{{ user.lastName }}</span>
            </div>
            <div class="py-3 flex items-center justify-between">
              <span class="text-neutral-400">{{ $t('Username') }}</span>
              <span class="text-neutral-100 font-medium">{{ user.userName }}</span>
            </div>
            <div class="py-3 flex items-center justify-between">
              <span class="text-neutral-400">{{ $t('Email') }}</span>
              <span class="text-neutral-100 font-medium">{{ user.email }}</span>
            </div>
          </div>

          <div class="mt-4 flex flex-col sm:flex-row gap-3 sm:justify-between">
            <button
              @click="goBack"
              class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                     border-1 border-neutral-700 bg-white/5 text-neutral-200
                     hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10">
              {{ $t('Back') }}
            </button>
          </div>
        </div>

        <!-- (Valikuline) Lisakaart tulevikuks: nt rollide kokkuvõte vms -->
        <!-- <div class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-4 sm:p-5"></div> -->
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
              See vaade kuvab <strong>valitud kasutaja</strong> põhiandmeid. Vaade on
              <strong>ainult lugemiseks</strong> – muuta ega kustutada siit ei saa.
            </p>
            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Väljad:</strong> <em>Email</em>, <em>Firstname</em>, <em>Lastname</em> ja
                <em>Username</em>.
              </li>
              <li>
                <strong>Laadimine:</strong> kui andmed on teel, kuvatakse “Loading…”. Kui andmeid ei ilmu,
                värskenda lehte või naase kasutajate loendisse.
              </li>
              <li>
                <strong>Rollid & õigused:</strong> rollide vaatamiseks/haldamiseks kasuta
                <em>Users and roles</em> vaadet.
              </li>
              <li>
                <strong>Privaatsus:</strong> vaata/jaga isikuandmeid ainult vastavate õigustega.
              </li>
            </ul>
            <p class="text-neutral-400 text-sm">
              Nipp: modaali saab sulgeda taustale klõpsates või ülanurga sulgemisnupust.
            </p>
          </div>

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
