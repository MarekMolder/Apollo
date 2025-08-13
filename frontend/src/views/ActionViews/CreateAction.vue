<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { ActionService } from '@/services/mvcServices/ActionService';
import { ActionTypeService } from '@/services/mvcServices/ActionTypeService';
import { ReasonService } from '@/services/mvcServices/ReasonService';
import { ProductService } from '@/services/mvcServices/ProductServices';
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService.ts';
import { useUserDataStore } from '@/stores/userDataStore';
import type { IAction } from '@/domain/logic/IAction';
import type { IReason } from '@/domain/logic/IReason';
import type { IProduct } from '@/domain/logic/IProduct';
import type { IActionType } from '@/domain/logic/IActionType';
import type { IStorageRoomEnriched } from "@/domain/logic/IStorageRoomEnriched.ts";
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';

// Services
const actionService = new ActionService();
const actionTypeService = new ActionTypeService();
const reasonService = new ReasonService();
const productService = new ProductService();
const storageRoomService = new StorageRoomService();

// Entity's
const actionTypes = ref<IActionType[]>([]);
const reasons = ref<IReason[]>([]);
const products = ref<IProduct[]>([]);
const storageRooms = ref<IStorageRoomEnriched[]>([]);

// Store
const store = useUserDataStore();

// Messages errors/success
const validationError = ref('');
const successMessage = ref('');

// Empty Action entity
const action = ref<IAction>({
  id: '',
  quantity: 0,
  status: 'Pending',
  actionTypeId: '',
  reasonId: '',
  productId: '',
  storageRoomId: '',
});

// admin / manager control
const isAdmin = computed(() =>
  store.roles.includes('admin') || store.roles.includes('manager')
);

// Get selects
onMounted(async () => {
  const invRes = await storageRoomService.getEnrichedStorageRooms();
  storageRooms.value = invRes.data ?? [];

  actionTypes.value = (await actionTypeService.getAllAsync()).data || [];
  reasons.value = (await reasonService.getAllAsync()).data || [];
  const allProducts = (await productService.getAllAsync()).data || [];
  products.value = allProducts.filter(p => !p.isComponent);

  if (!isAdmin.value) {
    const discard = actionTypes.value.find(a => a.name.toLowerCase() === 'maha kandmine');
    if (discard) {
      action.value.actionTypeId = discard.id;
    }
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

// Action create function
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
  <main class="flex flex-col items-center mt-4 px-4 sm:px-6 lg:px-8 py-10 text-white font-sans">
    <div class="w-full max-w-md sm:max-w-lg lg:max-w-2xl bg-[rgba(20,20,20,0.85)] backdrop-blur-md p-8 rounded-[16px] shadow-[0_0_16px_rgba(255,165,0,0.2)]">
      <h1 class="text-3xl font-bold text-center mb-6 text-orange-400">
        {{ $t('Create New') }} {{ $t('Action') }}
      </h1>

      <form @submit.prevent="createAction" class="flex flex-col gap-4">
        <div>
          <label for="actionType" class="font-semibold block mb-1">{{ $t('Action Type') }}</label>
          <Multiselect
            v-model="selectedActionType"
            :options="actionTypes"
            :custom-label="a => a.name"
            :track-by="'id'"
            :searchable="true"
            :close-on-select="true"
            :allow-empty="false"
            placeholder="-- Select Action Type --"
            class="text-black bg-white rounded-lg"
            :class="['custom-multiselect']"
            :disabled="!isAdmin"
          />
        </div>

        <div>
          <label for="reason" class="font-semibold block mb-1">{{ $t('Reason') }}</label>
          <Multiselect
            v-model="selectedReason"
            :options="reasons"
            :custom-label="r => r.description"
            :track-by="'id'"
            :searchable="true"
            :close-on-select="true"
            :allow-empty="false"
            placeholder="-- Select Reason --"
            class="text-black bg-white rounded-lg"
            :class="['custom-multiselect']"
          />
        </div>

        <div>
          <label for="product" class="font-semibold block mb-1">{{ $t('Product') }}</label>
          <Multiselect
            v-model="selectedProduct"
            :options="products"
            :custom-label="p => p.name"
            :track-by="'id'"
            :searchable="true"
            :close-on-select="true"
            :allow-empty="false"
            placeholder="-- Select Product --"
            class="text-black bg-white rounded-lg"
            :class="['custom-multiselect']"
          />
        </div>

        <div>
          <label for="quantity" class="font-semibold block mb-1">{{ $t('Quantity') }}
            <span v-if="selectedProduct">({{ selectedProduct.unit }})</span>
          </label>
          <input
            id="quantity"
            type="number"
            v-model.number="action.quantity"
            :step="selectedProduct && selectedProduct.unit === 'tk' ? 1 : 'any'"
            min="0"
            inputmode="decimal"
            required
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          />
        </div>

        <div>
          <label for="storageRoom" class="font-semibold block mb-1">{{ $t('StorageRoom') }}</label>
          <Multiselect
            v-model="selectedStorageRoom"
            :options="storageRooms"
            :custom-label="s => s.name"
            :track-by="'id'"
            :searchable="true"
            :close-on-select="true"
            :allow-empty="false"
            placeholder="-- Select Storage Room --"
            class="text-black bg-white rounded-lg"
            :class="['custom-multiselect']"
          />
        </div>

        <div v-if="validationError" class="text-red-400 text-center font-semibold pt-2">
          {{ validationError }}
        </div>

        <div v-if="successMessage" class="text-green-400 text-center font-semibold pt-2">
          {{ successMessage }}
        </div>

        <button
          type="submit"
          class="mt-4 w-full bg-gradient-to-r from-orange-500 to-yellow-400 hover:from-orange-400 hover:to-yellow-300 text-white font-bold py-2 rounded-lg transition-all"
        >
          {{ $t('Create') }}
        </button>
      </form>
    </div>
  </main>
</template>

<style>
.custom-multiselect {
  @apply w-full bg-zinc-800 text-black rounded-lg border border-neutral-700 shadow-sm focus:ring-1 focus:ring-orange-400 focus:border-orange-400 transition-all;
}

.custom-multiselect .multiselect__content-wrapper {
  @apply bg-zinc-900 border border-neutral-700 rounded-lg shadow-lg max-h-60 overflow-auto z-50;
}

.custom-multiselect .multiselect__option {
  @apply px-4 py-2 hover:bg-zinc-700 cursor-pointer transition text-black;
}

.custom-multiselect .multiselect__option--selected {
  @apply bg-orange-500 text-black;
}

.custom-multiselect .multiselect__option--highlight {
  @apply bg-zinc-700;
}

.custom-multiselect .multiselect__placeholder,
.custom-multiselect .multiselect__single {
  @apply text-black;
}

.custom-multiselect .multiselect__select {
  @apply text-black;
}

.custom-multiselect .multiselect__input {
  @apply bg-transparent text-black placeholder-black;
}

.custom-multiselect.multiselect--disabled {
  @apply bg-zinc-700 opacity-50 cursor-not-allowed;
}

</style>
