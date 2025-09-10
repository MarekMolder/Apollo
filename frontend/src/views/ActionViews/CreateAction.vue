<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { ActionService } from '@/services/mvcServices/ActionService';
import { ActionTypeService } from '@/services/mvcServices/ActionTypeService';
import { ReasonService } from '@/services/mvcServices/ReasonService';
import { ProductService } from '@/services/mvcServices/ProductServices';
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService.ts';
import type { IAction } from '@/domain/logic/IAction';
import type { IReason } from '@/domain/logic/IReason';
import type { IProduct } from '@/domain/logic/IProduct';
import type { IActionType } from '@/domain/logic/IActionType';
import type { IStorageRoomEnriched } from "@/domain/logic/IStorageRoomEnriched.ts";
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import { useSidebarStore } from "@/stores/sidebarStore";

// ---------------- Services ----------------
const actionService = new ActionService();
const actionTypeService = new ActionTypeService();
const reasonService = new ReasonService();
const productService = new ProductService();
const storageRoomService = new StorageRoomService();

// ---------------- Entities ----------------
const actionTypes = ref<IActionType[]>([]);
const reasons = ref<IReason[]>([]);
const products = ref<IProduct[]>([]);
const storageRooms = ref<IStorageRoomEnriched[]>([]);

  // ---------------- Store and drawer ----------------
const sidebarStore = useSidebarStore();
const showHelp = ref(false);

// ---------------- Messages errors/success ----------------
const validationError = ref('');
const successMessage = ref('');

// ---------------- Empty Action entity ----------------
const action = ref<IAction>({
  id: '',
  quantity: 0,
  status: 'Pending',
  actionTypeId: '',
  reasonId: '',
  productId: '',
  storageRoomId: '',
});

// ---------------- Fetch ----------------
onMounted(async () => {
  const invRes = await storageRoomService.getEnrichedStorageRooms();
  storageRooms.value = invRes.data ?? [];

  actionTypes.value = (await actionTypeService.getAllAsync()).data || [];
  reasons.value = (await reasonService.getAllAsync()).data || [];
  const allProducts = (await productService.getAllAsync()).data || [];
  products.value = allProducts;

  const discard = actionTypes.value.find(a => a.name.toLowerCase() === 'maha kandmine');
    if (discard) {
      action.value.actionTypeId = discard.id;
    }
});

const selectedProduct = computed({
  get: () => products.value.find(p => p.id === action.value.productId) || null,
  set: (product: IProduct | null) => {
    action.value.productId = product?.id ?? '';
  },
});

const selectedActionType = computed({
  get: () => actionTypes.value.find(a => a.id === action.value.actionTypeId) || null,
  set: (val: IActionType | null) => {
    action.value.actionTypeId = val?.id ?? '';
  }
});

const selectedReason = computed({
  get: () => reasons.value.find(r => r.id === action.value.reasonId) || null,
  set: (val: IReason | null) => {
    action.value.reasonId = val?.id ?? '';
  }
});

const selectedStorageRoom = computed({
  get: () => storageRooms.value.find(s => s.id === action.value.storageRoomId) || null,
  set: (val: IStorageRoomEnriched | null) => {
    action.value.storageRoomId = val?.id ?? '';
  }
});

// ---------------- Action create function ----------------
const createAction = async () => {
  validationError.value = '';
  successMessage.value = '';

  if (!action.value.quantity || isNaN(action.value.quantity)) {
    validationError.value = 'Quantity is required and must be a number.';
    return;
  }

  try {
    const { id, ...rest } = action.value;
    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as IAction;

    const result = await actionService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ Action has been successfully created!';
    }
  } catch (error) {
    console.error('Error creating action:', error);
  }
};
</script>

<template>
  <main
    :class="[
      'transition-all duration-300 p-4 sm:p-6 lg:p-8 text-white max-w-screen-2xl',
      sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
    ]"
  >
    <!-- HEADER -->
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
           drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]
           relative inline-block"
      >
    <span
      class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200
             bg-clip-text text-transparent"
    >
      {{ $t('Discard items') }}
    </span>
      </h1>

      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
    </section>

    <!-- Form -->
    <section class="mx-auto w-full max-w-xl sm:max-w-2xl">
      <div class="rounded-[16px] p-6 sm:p-8 bg-[rgba(25,25,25,0.4)] backdrop-blur-xl border-1 border-neutral-700 shadow-[inset_0_0_20px_rgba(255,165,0,0.03),_0_8px_24px_rgba(0,0,0,0.4)]">
        <form @submit.prevent="createAction" class="space-y-6">

          <!-- Product -->
          <div>
            <label class="mb-2 block text-medium font-medium text-neutral-300">
              {{ $t('Product') }}
            </label>
            <Multiselect
              v-model="selectedProduct"
              :options="products"
              :custom-label="p => p.name"
              track-by="id"
              :searchable="true"
              :close-on-select="true"
              :allow-empty="false"
              placeholder="-- Select Product --"
              class="multiselect-dark"
            />
          </div>

          <!-- Action type -->
          <div>
            <label class="mb-2 block text-medium font-medium text-neutral-300">
              {{ $t('Action type') }}
            </label>
            <Multiselect
              v-model="selectedActionType"
              :options="actionTypes"
              :custom-label="a => a.name"
              track-by="id"
              :searchable="true"
              :close-on-select="true"
              :allow-empty="false"
              placeholder="-- Select Action Type --"
              class="multiselect-dark"
            />
          </div>

          <!-- Reason -->
          <div>
            <label class="mb-2 block text-medium font-medium text-neutral-300">
              {{ $t('Reason') }}
            </label>
            <Multiselect
              v-model="selectedReason"
              :options="reasons"
              :custom-label="r => r.description"
              track-by="id"
              :searchable="true"
              :close-on-select="true"
              :allow-empty="false"
              placeholder="-- Select Reason --"
              class="multiselect-dark"
            />
          </div>

          <!-- Quantity -->
          <div>
            <label class="mb-2 block text-medium font-medium text-neutral-300">
              {{ $t('Quantity') }}
              <span v-if="selectedProduct" class="text-neutral-400">({{ selectedProduct.unit }})</span>
            </label>
            <input
              id="quantity"
              type="number"
              v-model.number="action.quantity"
              :step="selectedProduct && selectedProduct.unit === 'tk' ? 1 : 'any'"
              min="0"
              inputmode="decimal"
              required
              placeholder="0"
              class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 py-3 text-base text-white placeholder-neutral-500 outline-none transition focus:border-[#ffaa33] focus:ring-2 focus:ring-[#ffaa33]/35"
            />
          </div>

          <!-- Storage room -->
          <div>
            <label class="mb-2 block text-medium font-medium text-neutral-300">
              {{ $t('Storage room') }}
            </label>
            <Multiselect
              v-model="selectedStorageRoom"
              :options="storageRooms"
              :custom-label="s => s.name"
              track-by="id"
              :searchable="true"
              :close-on-select="true"
              :allow-empty="false"
              placeholder="-- Select Storage Room --"
              class="multiselect-dark"
            />
          </div>

          <!-- Messages -->
          <p v-if="validationError" class="text-rose-400 text-center font-medium">
            {{ validationError }}
          </p>
          <p v-if="successMessage" class="text-emerald-400 text-center font-medium">
            {{ successMessage }}
          </p>

          <!-- Actions -->
          <div class="pt-2 flex flex-col sm:flex-row gap-3">
            <button
              type="button"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-5 py-3 text-sm font-medium text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="
                () => {
                  action.quantity = 0;
                  action.actionTypeId = '';
                  action.reasonId = '';
                  action.productId = '';
                  action.storageRoomId = '';
                }
              "
            >
              {{ $t('Reset') }}
            </button>

            <button
              type="submit"
              class="w-full sm:flex-1 inline-flex items-center justify-center rounded-xl
               bg-white/5
               text-neutral-200 px-6 py-3 text-sm font-medium
               border-1 border-neutral-700
               shadow-[0_0_8px_rgba(0,0,0,0.4)]
               transition-all duration-300
              hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15"
            >
              {{ $t('Create') }}
            </button>
          </div>
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
              See vaade on mõeldud <strong>mahakandmiste</strong> (Discard) lisamiseks. Täida allolev vorm ja salvesta,
              et kande kirje tekiks süsteemi ning kajastuks ladude statistikas.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Product:</strong> vali toode, mida maha kantakse.
                (Loendis on ainult <em>mitte-komponenttooted</em>.)
              </li>
              <li>
                <strong>Action Type:</strong> toimingu tüüp. <em>Admin/manager</em> saab seda muuta; tavakasutajal
                võib see olla eelnevalt lukus väärtusele “maha kandmine”.
              </li>
              <li>
                <strong>Reason:</strong> vali põhjendus (nt aegunud, kahjustatud jms).
              </li>
              <li>
                <strong>Quantity:</strong> sisesta kogus valitud toote ühikus
                (<span class="text-neutral-400">välja järel näidatakse ühikut</span>).
                Kui ühik on <code>tk</code>, sisend on täisarv; muudel juhtudel lubatakse ka komakohad.
              </li>
              <li>
                <strong>Storage Room:</strong> vali ladu/laoruum, kust kaubad maha kantakse.
              </li>
              <li>
                <strong>Reset:</strong> puhastab vormi väljad.
              </li>
              <li>
                <strong>Create:</strong> salvestab kande. Vigade korral kuvatakse teade punasena; õnnestumisel roheline kinnitus.
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: koguse sisestamisel kasuta komakoha jaoks punkt<i>(.)</i>.
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

<style scoped>
/* --- vue-multiselect: tume, puhas --- */
:deep(.multiselect-dark) {
  @apply w-full rounded-xl border border-white/10 bg-neutral-900/70 text-white shadow-sm transition;
}

/* Konteiner teksti vertikaalseks keskjoondamiseks */
:deep(.multiselect-dark .multiselect__tags) {
  @apply flex items-center min-h-[44px] rounded-xl border-0 bg-transparent px-3 py-0;
}

/* Placeholder + valitud teksti joondus ja läbipaistev taust */
:deep(.multiselect-dark .multiselect__placeholder),
:deep(.multiselect-dark .multiselect__single) {
  /* puhastame vaikimisi padjad/taustad ja paneme line-height sama mis kõrgus */
  @apply block p-0 m-0 bg-transparent text-neutral-300 leading-[44px];
}

/* Sisend (otsing) sama kõrgusega, keskel */
:deep(.multiselect-dark .multiselect__input) {
  @apply bg-transparent text-white placeholder-neutral-500 leading-[44px] p-0 m-0;
}

/* Nool & clear ikoonid */
:deep(.multiselect-dark .multiselect__select),
:deep(.multiselect-dark .multiselect__clear) {
  @apply text-neutral-400 hover:text-white;
}

/* Aktiivne rõngas */
:deep(.multiselect-dark.multiselect--active .multiselect__tags) {
  @apply ring-2 ring-[#ffaa33]/35 outline-none border-[#ffaa33];
}

/* Dropdown – tume menüü */
:deep(.multiselect-dark .multiselect__content-wrapper) {
  @apply mt-2 rounded-xl border border-white/10 bg-neutral-950/95 backdrop-blur supports-[backdrop-filter]:bg-neutral-950/80 shadow-2xl max-h-64;
}
:deep(.multiselect-dark .multiselect__content) { @apply py-1; }
:deep(.multiselect-dark .multiselect__option) {
  @apply px-4 py-2 text-neutral-200 cursor-pointer transition;
}
/* Hover */
:deep(.multiselect-dark .multiselect__option--highlight) { @apply bg-white/10; }
/* Valitud valik – mitte hele, vaid õrn tume + bränditekst */
:deep(.multiselect-dark .multiselect__option--selected) {
  @apply bg-white/[0.06] text-[#ffaa33];
}

/* Disabled */
:deep(.multiselect-dark.multiselect--disabled) { @apply opacity-60 cursor-not-allowed; }

/* --- Inputs / nupud: vähem heledaid jooni --- */
:deep(input[type="number"]) {
  /* vaikimisi piirjoon mahedamaks (kui mitte classis üle kirjutatud) */
  @apply border-white/5;
}
</style>
