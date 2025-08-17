<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { IdentityService } from '@/services/IdentityService'
import { useSidebarStore } from '@/stores/sidebarStore'
const sidebarStore = useSidebarStore()
const showHelp = ref(false);

// Services
const identityService = new IdentityService()

// Router
const route = useRoute()

// Empty user
const user = ref<{
  id: string
  email: string
  firstName: string
  lastName: string
  userName: string
} | null>(null)

// Get User details
onMounted(async () => {
  try {
    const id = route.params.id as string
    user.value = await identityService.getUserById(id)
  } catch (err) {
    console.error('Failed to load user:', err)
  }
})
</script>

<template>
  <main
    class="w-full h-full flex justify-center items-center px-4 py-12 sm:px-6 sm:py-16 md:px-12 md:py-20 text-white font-['Segoe_UI']"
  >
    <div
      class="w-full max-w-[500px] bg-[rgba(20,20,20,0.85)] rounded-[16px] p-5 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md"
    >
      <h2 class="text-[#ffaa33] text-center text-xl sm:text-2xl font-semibold mb-6">
        {{ $t('User Details') }}
      </h2>

      <div
        v-if="user"
        class="grid grid-cols-1 sm:grid-cols-[140px_1fr] gap-y-3 gap-x-4 text-left"
      >
        <div class="text-white font-medium">{{ $t('Email') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.email }}</div>

        <div class="text-white font-medium">{{ $t('Firstname') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.firstName }}</div>

        <div class="text-white font-medium">{{ $t('Lastname') }}:</div>
        <div class="text-gray-200 border-b border-[#333] pb-1">{{ user.lastName }}</div>

        <div class="text-white font-medium">{{ $t('Username') }}:</div>
        <div class="text-gray-200">{{ user.userName }}</div>
      </div>

      <p v-else class="italic text-gray-400 text-center mt-4">{{ $t('Loading...') }}</p>
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
              Sellel lehel saad <strong>otsida</strong>, <strong>luua</strong>, <strong>muuta</strong> ja
              <strong>kustutada</strong> tarnijaid ning vaadata, millised tooted on konkreetse tarnijaga seotud.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li><strong>Otsing:</strong> ülal vasakul “Search by name” filtreerib kaarte nime järgi.</li>
              <li><strong>Uus tarnija:</strong> klõpsa “New Supplier”, täida vorm ja salvesta.</li>
              <li><strong>Muuda:</strong> kaardil <em>Edit</em> avab vormi olemasoleva tarnija muutmiseks.</li>
              <li><strong>Tooted:</strong> <em>Products</em> näitab valitud tarnija tooteid.</li>
              <li><strong>Kustuta:</strong> prügikasti ikoon kaardi paremas ülanurgas.</li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaalid saab sulgeda ka klõpsates tumedal taustal või vajutades sulgemisnupule.
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

