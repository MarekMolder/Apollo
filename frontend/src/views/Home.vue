<script setup>
import PendingActionsCard from "@/components/PendingActionsCard.vue";
import ProblematicProducts from "@/components/ProblematicProducts.vue";
import TopUsersByRemoveCard from "@/components/TopUsersByRemoveCard.vue";
import UserListCard from "@/components/UserListCard.vue";
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 px-4 sm:px-6 lg:px-8 py-6 sm:py-8 text-white',
      sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
    ]"
  >
    <!-- Pealkiri -->
    <section class="mb-6 text-center">
      <h1 class="text-3xl sm:text-4xl font-[Playfair_Display] font-bold text-neutral-100">
        {{ $t('Dashboard') }}
      </h1>
      <div class="mx-auto mt-3 h-px w-64 max-w-full bg-gradient-to-r from-transparent via-white/15 to-transparent"></div>
    </section>

    <!-- Üks kaart ühe rea peal, laius vastavalt ekraanile -->
    <section class="mx-auto w-full">
      <div class="space-y-4 sm:space-y-5 lg:space-y-6">
        <!-- Iga ümbris keskendab kaardi ja piirab laiuse: min 22rem, eelistatult 92vw, max 72rem -->
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
  </main>
</template>

