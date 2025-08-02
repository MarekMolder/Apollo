<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import router from '@/router';
import { ActionService } from '@/services/mvcServices/ActionService';
import { ActionTypeService } from '@/services/mvcServices/ActionTypeService';
import { ReasonService } from '@/services/mvcServices/ReasonService';
import { SupplierService } from '@/services/mvcServices/SupplierService';
import { ProductService } from '@/services/mvcServices/ProductServices';
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService.ts';
import { useUserDataStore } from '@/stores/userDataStore';
import type { IAction } from '@/domain/logic/IAction';
import type { IReason } from '@/domain/logic/IReason';
import type { IProduct } from '@/domain/logic/IProduct';
import type { ISupplier } from '@/domain/logic/ISupplier';
import type { IActionType } from '@/domain/logic/IActionType';
import type { IStorageRoomEnriched } from "@/domain/logic/IStorageRoomEnriched.ts";

const actionService = new ActionService();
const actionTypeService = new ActionTypeService();
const reasonService = new ReasonService();
const supplierService = new SupplierService();
const productService = new ProductService();
const storageRoomService = new StorageRoomService();

const store = useUserDataStore();

const isAdmin = computed(() =>
  store.roles.includes('admin') || store.roles.includes('manager')
);

const validationError = ref('');
const successMessage = ref('');

const action = ref<IAction>({
  id: '',
  quantity: 0,
  status: 'Pending',
  actionTypeId: '',
  reasonId: '',
  supplierId: '',
  productId: '',
  storageRoomId: '',
});

const actionTypes = ref<IActionType[]>([]);
const reasons = ref<IReason[]>([]);
const products = ref<IProduct[]>([]);
const suppliers = ref<ISupplier[]>([]);
const storageRooms = ref<IStorageRoomEnriched[]>([]);

onMounted(async () => {
  const invRes = await storageRoomService.getEnrichedStorageRooms();
  storageRooms.value = invRes.data ?? [];

  actionTypes.value = (await actionTypeService.getAllAsync()).data || [];
  reasons.value = (await reasonService.getAllAsync()).data || [];
  products.value = (await productService.getAllAsync()).data || [];
  suppliers.value = (await supplierService.getAllAsync()).data || [];

  if (!isAdmin.value) {
    const discard = actionTypes.value.find(a => a.name.toLowerCase() === 'maha kandmine');
    if (discard) {
      action.value.actionTypeId = discard.id;
    }
  }
});

const doAdd = async () => {
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

const selectedActionType = computed(() => {
  return actionTypes.value.find(type => type.id === action.value.actionTypeId);
});

const isTellimine = computed(() => selectedActionType.value?.name.toLowerCase() === 'tellimine');
const isMahakandmine = computed(() => selectedActionType.value?.name.toLowerCase() === 'maha kandmine');
</script>

<template>
  <main class="flex flex-col items-center mt-4 px-6 py-10 text-white font-sans">
    <div class="w-full max-w-2xl bg-[rgba(20,20,20,0.85)] backdrop-blur-md p-8  rounded-[16px] shadow-[0_0_16px_rgba(255,165,0,0.2)]">
      <h1 class="text-3xl font-bold text-center mb-6 text-orange-400">
        {{ $t('Create New') }} {{ $t('Action') }}
      </h1>

      <form @submit.prevent="doAdd" class="flex flex-col gap-4">
        <div>
          <label for="quantity" class="font-semibold block mb-1">{{ $t('Quantity') }}</label>
          <input
            id="quantity"
            type="number"
            v-model="action.quantity"
            required
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          />
        </div>

        <div>
          <label for="actionType" class="font-semibold block mb-1">{{ $t('Action Type') }}</label>
          <select
            id="actionType"
            v-model="action.actionTypeId"
            :disabled="!isAdmin"
            required
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition0"
          >
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Action Type') }} --</option>
            <option v-for="type in actionTypes" :key="type.id" :value="type.id">
              {{ type.name }}
            </option>
          </select>
        </div>

        <div v-if="isMahakandmine">
          <label for="reason" class="font-semibold block mb-1">{{ $t('Reason') }}</label>
          <select
            id="reason"
            v-model="action.reasonId"
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          >
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Reason') }} --</option>
            <option v-for="reason in reasons" :key="reason.id" :value="reason.id">
              {{ reason.description }}
            </option>
          </select>
        </div>

        <div v-if="isTellimine">
          <label for="supplier" class="font-semibold block mb-1">{{ $t('Supplier') }}</label>
          <select
            id="supplier"
            v-model="action.supplierId"
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          >
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Supplier') }} --</option>
            <option v-for="supplier in suppliers" :key="supplier.id" :value="supplier.id">
              {{ supplier.name }}
            </option>
          </select>
        </div>

        <div>
          <label for="product" class="font-semibold block mb-1">{{ $t('Product') }}</label>
          <select
            id="product"
            v-model="action.productId"
            required
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          >
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Product') }} --</option>
            <option v-for="product in products" :key="product.id" :value="product.id">
              {{ product.name }}
            </option>
          </select>
        </div>

        <div>
          <label for="storageRoom" class="font-semibold block mb-1">{{ $t('StorageRoom') }}</label>
          <select
            id="storageRoom"
            v-model="action.storageRoomId"
            required
            class="w-full px-3 py-2 rounded-lg bg-zinc-800 border-1 border-neutral-700 focus:outline-none focus:border-[#ffaa33] transition"
          >
            <option disabled value="">-- {{ $t('Select') }} {{ $t('StorageRoom') }} --</option>
            <option v-for="room in storageRooms" :key="room.id" :value="room.id">
              {{ room.name }}
            </option>
          </select>
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
