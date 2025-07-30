<script setup lang="ts">
import {computed, onMounted, ref} from 'vue';
import router from '@/router';
import { ActionService } from '@/services/mvcServices/ActionService';
import { ActionTypeService } from '@/services/mvcServices/ActionTypeService';
import { ReasonService } from '@/services/mvcServices/ReasonService';
import { SupplierService } from '@/services/mvcServices/SupplierService';
import { ProductService } from '@/services/mvcServices/ProductServices';
import { StorageRoomService } from '@/services/mvcServices/StorageRoomService.ts';
import type { IAction } from '@/domain/logic/IAction';
import type { IReason } from '@/domain/logic/IReason';
import type { IProduct } from '@/domain/logic/IProduct';
import type { ISupplier } from '@/domain/logic/ISupplier';
import type { IActionType } from '@/domain/logic/IActionType';
import { useUserDataStore } from '@/stores/userDataStore';
import type {IStorageRoom} from "@/domain/logic/IStorageRoom.ts";
import type {IStorageRoomEnriched} from "@/domain/logic/IStorageRoomEnriched.ts";

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
  const visibleStorageRooms = invRes.data ?? [];

  storageRooms.value = visibleStorageRooms;

  actionTypes.value = (await actionTypeService.getAllAsync()).data || [];
  reasons.value = (await reasonService.getAllAsync()).data || [];
  const allProducts = (await productService.getAllAsync()).data || [];
  products.value = allProducts.filter(p => !p.isComponent);
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
  <main class="action-wrapper">
    <div class="action-box">
      <h1 class="title">{{ $t('Create New') }} {{ $t('Action') }}</h1>

      <form @submit.prevent="doAdd">
        <label for="quantity">{{ $t('Quantity') }}</label>
        <input id="quantity" type="number" v-model="action.quantity" required />

        <label for="actionType">{{ $t('Action Type') }}</label>
        <select id="actionType" v-model="action.actionTypeId" :disabled="!isAdmin" required>
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Action Type') }} --</option>
          <option
            v-for="actionType in actionTypes"
            :key="actionType.id"
            :value="actionType.id"
          >
            {{ actionType.name }}
          </option>
        </select>

        <template v-if="isMahakandmine">
          <label for="reason">{{ $t('Reason') }}</label>
          <select id="reason" v-model="action.reasonId">
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Reason') }} --</option>
            <option v-for="reason in reasons" :key="reason.id" :value="reason.id">
              {{ reason.description }}
            </option>
          </select>
        </template>

        <template v-if="isTellimine">
          <label for="supplier">{{ $t('Supplier') }}</label>
          <select id="supplier" v-model="action.supplierId">
            <option disabled value="">-- {{ $t('Select') }} {{ $t('Supplier') }} --</option>
            <option v-for="supplier in suppliers" :key="supplier.id" :value="supplier.id">
              {{ supplier.name }}
            </option>
          </select>
        </template>

        <label for="product">{{ $t('Product') }}</label>
        <select id="product" v-model="action.productId" required>
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Product') }} --</option>
          <option v-for="product in products" :key="product.id" :value="product.id">
            {{ product.name }}
          </option>
        </select>

        <label for="product">{{ $t('StorageRoom') }}</label>
        <select id="product" v-model="action.storageRoomId" required>
          <option disabled value="">-- {{ $t('Select') }} {{ $t('StorageRoom') }} --</option>
          <option v-for="storageRoom in storageRooms" :key="storageRoom.id" :value="storageRoom.id">
            {{ storageRoom.name }}
          </option>
        </select>

        <div class="text-danger" v-if="validationError">{{ validationError }}</div>
        <div class="text-success" v-if="successMessage">{{ successMessage }}</div>

        <button type="submit" class="create-button">{{ $t('Create') }}</button>
      </form>
    </div>
  </main>
</template>

<style scoped>
.action-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 2vh;
  padding: 2rem;
  font-family: 'Segoe UI', sans-serif;
  color: #f4f4f4;
}

.action-box {
  background: rgba(20, 20, 20, 0.85);
  backdrop-filter: blur(6px);
  padding: 2rem;
  border-radius: 12px;
  width: 100%;
  max-width: 600px;
  box-shadow: 0 0 18px rgba(255, 165, 0, 0.4);
  border: 2px solid orange;
}

.title {
  font-size: 2rem;
  text-align: center;
  margin-bottom: 1.5rem;
  color: orange;
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

label {
  font-weight: bold;
  margin-bottom: 0.25rem;
  color: #f0f0f0;
}

input,
select {
  width: 100%;
  padding: 0.6rem 0.75rem;
  border: 1px solid #444;
  border-radius: 8px;
  font-size: 1rem;
  background-color: #2a2a2a;
  color: white;
  appearance: none;
  transition: border-color 0.3s ease, background-color 0.3s ease;
}

input:focus,
select:focus {
  outline: none;
  border-color: orange;
  background-color: #1a1a1a;
}

option {
  background-color: #1a1a1a;
  color: white;
}

.create-button {
  margin-top: 1rem;
  background: linear-gradient(to right, #ff8c00, #ffa500);
  border: none;
  border-radius: 8px;
  padding: 0.75rem;
  width: 100%;
  color: white;
  font-weight: bold;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.3s;
}

.create-button:hover {
  background: linear-gradient(to right, #ffa500, #ffcc00);
}

.text-danger,
.text-success {
  text-align: center;
  font-weight: bold;
  padding-top: 0.5rem;
}

.text-danger {
  color: #ff4d4d;
}

.text-success {
  color: #28a745;
}
</style>
