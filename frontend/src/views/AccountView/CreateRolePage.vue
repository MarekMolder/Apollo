<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RoleService } from '@/services/RoleService'
import type { AppRole } from '@/domain/logic/AppRole'
import { useSidebarStore } from '@/stores/sidebarStore'

// // ---------------- Services ----------------
const roleService = new RoleService()

// ---------------- Entities ----------------
const roles = ref<AppRole[]>([])

// ---------------- Store, drawer and role string ----------------
const sidebarStore = useSidebarStore()
const showHelp = ref(false);
const newRoleName = ref('')

// ---------------- Messages errors/success ----------------
const validationError = ref('')
const successMessage = ref('')

// ---------------- Fetch ----------------
const fetchRoles = async () => {
  try {
    roles.value = await roleService.getAllRoles()
  } catch (e: any) {
    validationError.value = e.message
  }
}

onMounted(fetchRoles)

// ---------------- Role create function  ----------------
const createRole = async () => {
  validationError.value = ''
  successMessage.value = ''

  if (!newRoleName.value.trim()) {
    validationError.value = 'Role name cant be empty'
    return
  }

  const result = await roleService.createRole(newRoleName.value.trim())
  if (result.errors!.length) {
    validationError.value = result.errors!.join(', ')
  } else {
    successMessage.value = result.data!
    newRoleName.value = ''
    await fetchRoles()
  }
}
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
      {{ $t('Roles') }}
    </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-64 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">{{ $t('Manage access roles for your system') }}</p>
    </section>

    <!-- Form -->
    <div class="rounded-xl border border-neutral-700 bg-neutral-900/60 p-5 sm:p-6 shadow-[0_0_0_1px_rgba(255,255,255,0.02),_0_8px_24px_rgba(0,0,0,0.35)] backdrop-blur-xl mb-8">
      <form @submit.prevent="createRole" class="flex flex-col sm:flex-row gap-4">
        <input
          v-model="newRoleName"
          type="text"
          :placeholder="$t('Enter new role name') as string"
          class="flex-1 rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                 placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
        />
        <button
          type="submit"
          class="inline-flex items-center justify-center rounded-xl px-5 h-11 text-sm font-semibold
                 border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                 shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                 hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition w-full sm:w-auto"
        >
          + {{ $t('Create') }}
        </button>
      </form>

      <p
        v-if="validationError"
        class="mt-4 text-medium font-medium px-4 py-2 rounded-md bg-red-500/10 border border-red-500/20 text-red-400"
      >
        {{ validationError }}
      </p>
      <p
        v-if="successMessage"
        class="mt-4 text-medium font-medium px-4 py-2 rounded-md bg-emerald-500/10 border border-emerald-500/20 text-emerald-400"
      >
        {{ successMessage }}
      </p>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto rounded-xl border border-neutral-700 bg-neutral-900/60 shadow-inner backdrop-blur-xl">
      <table class="w-full min-w-[400px] text-left text-medium">
        <thead class="bg-neutral-800/50 text-neutral-300">
        <tr>
          <th class="py-3 px-4 font-medium">{{ $t('ID') }}</th>
          <th class="py-3 px-4 font-medium">{{ $t('Role name') }}</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="role in roles" :key="role.id" class="hover:bg-white/5 transition">
          <td class="py-3 px-4 border-t border-white/10">{{ role.id }}</td>
          <td class="py-3 px-4 border-t border-white/10">{{ role.name }}</td>
        </tr>
        </tbody>
      </table>
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
          <!-- Header -->
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

          <!-- Body -->
          <div class="mt-5 space-y-4 text-neutral-200 leading-relaxed">
            <p>
              Sellel lehel näed kõiki süsteemi <strong>rolle</strong> ja saad luua uusi rollinimesid,
              mida saab kasutada ligipääsu määramiseks (nt vaadetes või ladude <em>Allowed Roles</em> väljal).
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Uue rolli loomine:</strong> sisesta nimi väljale <em>Enter new role name</em> ja vajuta <em>Create</em>.
                Tühja nime ei salvestata; ees- ja lõputühikud eemaldatakse automaatselt.
              </li>
              <li>
                <strong>Tulemuse kuva:</strong> õnnestumisel ilmub roheline kinnitus ning roll lisatakse allolevasse tabelisse.
                Kui backend tagastab vead (nt nimi on juba kasutusel), kuvatakse punane veateade.
              </li>
              <li>
                <strong>Rollide loend:</strong> tabelis on näha <em>ID</em> ja <em>Role Name</em>.
                See on referentsloend – rollide kustutamine või ümbernimetamine selles vaates ei ole võimalik.
              </li>
              <li>
                <strong>Nime soovitused:</strong> hoia nimed lühikesed ja ühtlases vormis
                (nt <code>admin</code>, <code>manager</code>, <code>mustamae</code>); väldi tühikuid.
              </li>
              <li>
                <strong>Kus rolle kasutatakse:</strong> kasutajate rollidega seotud toimingud on lehel
                <em>Users and roles</em>; ladude ligipääsu piiramiseks saab rolle sisestada välja <em>Allowed Roles</em>.
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
              {{ $t('Got it') }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </main>
</template>
