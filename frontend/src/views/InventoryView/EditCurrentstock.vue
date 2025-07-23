<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";

import {CurrentStockService} from "@/services/mvcServices/CurrentStockService.ts";
import type {ICurrentStock} from "@/domain/logic/ICurrentStock.ts";

const route = useRoute();
const currentStockId = route.params.id as string;

const currentStockService = new CurrentStockService();

const currentStock = ref<ICurrentStock | null>(null);

const successMessage = ref("");
const validationError = ref("");

onMounted(async () => {
  const allCurrentStocks = await currentStockService.getAllAsync();
  currentStock.value = allCurrentStocks.data?.find(p => p.id === currentStockId) || null;
});

const doUpdate = async () => {
  if (!currentStock.value) return;
  successMessage.value = "";
  validationError.value = "";

  try {
    const result = await currentStockService.updateAsync(currentStock.value);
    if (result.errors?.length) {
      validationError.value = result.errors.join(", ");
    } else {
      successMessage.value = "✅ CurrentStock successfully updated!";
    }
  } catch (error) {
    console.error("Error updating currentStock:", error);
  }
};
</script>

<template>
  <main class="product-wrapper">
    <div class="product-box" v-if="currentStock">
      <h1 class="title">{{ $t('Edit') }} {{ $t('Current Stock') }}</h1>
      <form @submit.prevent="doUpdate">

        <label for="quantity">{{ $t('Quantity') }}</label>
        <input id="quantity" type="number" v-model="currentStock.quantity" required />

        <div class="text-danger" v-if="validationError">{{ validationError }}</div>
        <div class="text-success" v-if="successMessage">{{ successMessage }}</div>

        <button type="submit" class="create-button">{{ $t('Update') }}</button>
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
