<script setup>
import PendingActionsCard from "@/components/PendingActionsCard.vue";
import ProblematicProducts from "@/components/ProblematicProducts.vue";
import TopUsersByRemoveCard from "@/components/TopUsersByRemoveCard.vue";
import UserListCard from "@/components/UserListCard.vue";
import { useSidebarStore } from '@/stores/sidebarStore';
import {ref} from "vue";

// ---------------- Sidebar store ----------------
const sidebarStore = useSidebarStore();

// ---------------- Help Model ----------------
const showHelp = ref(false);
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 px-4 sm:px-6 lg:px-8 py-6 sm:py-8 text-white',
      sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
    ]"
  >
    <!-- HEADER -->
    <section class="mb-6 text-center">
      <h1 class="text-3xl sm:text-4xl font-[Playfair_Display] font-bold text-neutral-100">
        {{ $t('Dashboard') }}
      </h1>
      <div class="mx-auto mt-3 h-px w-64 max-w-full bg-gradient-to-r from-transparent via-white/15 to-transparent"></div>
    </section>

    <!-- Card container -->
    <section class="mx-auto w-full">
      <div class="space-y-4 sm:space-y-5 lg:space-y-6">
        <div class="mx-auto w-full max-w-[clamp(22rem,92vw,72rem)]">
          <PendingActionsCard />
        </div>

        <div class="mx-auto w-full max-w-[clamp(22rem,92vw,72rem)]">
          <ProblematicProducts />
        </div>

        <div class="mx-auto w-full max-w-[clamp(22rem,92vw,72rem)]">
          <TopUsersByRemoveCard />
        </div>

        <div class="mx-auto w-full max-w-[clamp(22rem,92vw,72rem)]">
          <UserListCard />
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
              title="Close"
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
              <li><strong>Otsing:</strong> ülal vasakul “Search by name” filtreerib tarnijaid nime järgi.</li>
              <li><strong>Uus tarnija:</strong> vajuta “New Supplier”, täida vorm ja salvesta.</li>
              <li><strong>Muuda:</strong> tarnija kaardil <em>Edit</em> avab vormi olemasoleva tarnija muutmiseks.</li>
              <li><strong>Tooted:</strong> <em>Products</em> näitab valitud tarnija tooteid.</li>
              <li><strong>Kustuta:</strong> prügikasti ikoon tarnija kaardi paremas ülanurgas kustutab tarnija.</li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: Vihje lehe saad sulgeda, kas klõpsates tumedale taustale või vajutades sulgemisnupule.
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

