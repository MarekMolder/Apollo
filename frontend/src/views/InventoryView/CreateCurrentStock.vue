<script setup lang="ts">

import {ProductService} from "@/services/mvcServices/ProductServices.ts";
import {useUserDataStore} from "@/stores/userDataStore.ts";
import {onMounted, ref} from "vue";
import type {IAction} from "@/domain/logic/IAction.ts";
import type {IProduct} from "@/domain/logic/IProduct.ts";
import type {IProductCategory} from "@/domain/logic/IProductCategory.ts";
import {ProductCategoryService} from "@/services/mvcServices/ProductCategoryService.ts";
import {CurrentStockService} from "@/services/mvcServices/CurrentStockService.ts";
import {StorageRoomService} from "@/services/mvcServices/StorageRoomService.ts";
import type {ICurrentStock} from "@/domain/logic/ICurrentStock.ts";
import type {IStorageRoom} from "@/domain/logic/IStorageRoom.ts";

const productService = new ProductService();
const currentStockService = new CurrentStockService();
const storageRoomService = new StorageRoomService();

const store = useUserDataStore();

const validationError = ref('');
const successMessage = ref('');

const currentStock = ref<ICurrentStock>({
  id: '',
  quantity: 0,
  updatedAt: '',
  productId: '',
  StorageRoomId: '',
});

const products = ref<IProduct[]>([]);
const currentStocks = ref<ICurrentStock[]>([]);
const storageRooms = ref<IStorageRoom[]>([]);

onMounted(async () => {
  products.value = (await productService.getAllAsync()).data || [];
  currentStocks.value = (await currentStockService.getAllAsync()).data || [];
  storageRooms.value = (await storageRoomService.getAllAsync()).data || [];
});

const doAdd = async () => {
  validationError.value = '';
  successMessage.value = '';

  try {
    const { id, ...rest } = currentStock.value;
    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as ICurrentStock;

    const result = await currentStockService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ CurrentStock has been successfully created!';
    }
  } catch (error) {
    console.error('Error creating CurrentStock:', error);
  }
};

</script>

<template>
  <main class="product-wrapper">
    <div class="product-box">
      <h1 class="title">{{ $t('Create new current stock') }}</h1>

      <form @submit.prevent="doAdd">
        <label for="quantity">{{ $t('Quantity') }}</label>
        <input id="quantity" type="number" v-model="currentStock.quantity" required />

        <label for="product">{{ $t('Product') }}</label>
        <select id="product" v-model="currentStock.productId">
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Product') }} --</option>
          <option v-for="product in products" :key="product.id" :value="product.id">
            {{ product.name }}
          </option>
        </select>

        <label for="productCategory">{{ $t('StorageRoom') }}</label>
        <select id="productCategory" v-model="currentStock.StorageRoomId">
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
.product-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 10vh;
  font-family: Arial, sans-serif;
}

.product-box {
  background-color: #1a1a1a;
  padding: 2rem;
  border-radius: 12px;
  width: 450px;
  box-shadow: 0 0 10px rgba(255, 165, 0, 0.6);
  border: 4px solid orange;
  color: white;
}

.title {
  font-size: 2rem;
  text-align: center;
  margin-bottom: 1.5rem;
  color: orange;
}

label {
  display: block;
  margin-top: 1rem;
  margin-bottom: 0.3rem;
  font-weight: bold;
}

input,
select {
  width: 100%;
  padding: 0.5rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  background-color: #fff7e6;
  color: #000;
}

.create-button {
  margin-top: 2rem;
  background: linear-gradient(to right, #ff8c00, #ffa500);
  border: none;
  border-radius: 6px;
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

.text-danger {
  color: #ff4d4d;
  margin-top: 1rem;
  font-weight: bold;
  text-align: center;
}

.text-success {
  color: #28a745;
  margin-top: 1rem;
  font-weight: bold;
  text-align: center;
}


</style>
