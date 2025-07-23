<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";

import {SupplierService} from "@/services/mvcServices/SupplierService.ts";
import {AddressService} from "@/services/mvcServices/AddressService.ts";
import type {ISupplier} from "@/domain/logic/ISupplier.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";

const route = useRoute();
const supplierId = route.params.id as string;

const supplierService = new SupplierService();
const addressService = new AddressService();

const supplier = ref<ISupplier | null>(null);
const address = ref<IAddress[]>([]);
const successMessage = ref("");
const validationError = ref("");

onMounted(async () => {
  const allSuppliers = await supplierService.getAllAsync();
  supplier.value = allSuppliers.data?.find(p => p.id === supplierId) || null;

  const addresses = await addressService.getAllAsync();
  address.value = addresses.data || [];
});

const doUpdate = async () => {
  if (!supplier.value) return;
  successMessage.value = "";
  validationError.value = "";

  try {
    const result = await supplierService.updateAsync(supplier.value);
    if (result.errors?.length) {
      validationError.value = result.errors.join(", ");
    } else {
      successMessage.value = "✅ Supplier successfully updated!";
    }
  } catch (error) {
    console.error("Error updating supplier:", error);
  }
};
</script>

<template>
  <main class="product-wrapper">
    <div class="product-box" v-if="supplier">
      <h1 class="title">{{ $t('Edit') }} {{ $t('Supplier') }}</h1>
      <form @submit.prevent="doUpdate">
        <label for="name">{{ $t('Name') }}</label>
        <input id="name" type="text" v-model="supplier.name" required />

        <label for="telephoneNr">{{ $t('Telephone nr') }}</label>
        <input id="telephoneNr" type="text" v-model="supplier.telephoneNr" required />

        <label for="email">{{ $t('Email') }}</label>
        <input id="email" type="text" v-model="supplier.email" required />

        <label for="address">{{ $t('Address') }}</label>
        <select id="address" v-model="supplier.addressId">
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Address') }} --</option>
          <option v-for="address in address" :key="address.id" :value="address.id">
            {{ address.name }}
          </option>
        </select>

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
