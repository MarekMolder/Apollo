<script setup lang="ts">


import {useUserDataStore} from "@/stores/userDataStore.ts";
import {onMounted, ref} from "vue";
import {SupplierService} from "@/services/mvcServices/SupplierService.ts";
import {AddressService} from "@/services/mvcServices/AddressService.ts";
import type {ISupplier} from "@/domain/logic/ISupplier.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";

const supplierService = new SupplierService();
const addressService = new AddressService();

const store = useUserDataStore();

const validationError = ref('');
const successMessage = ref('');

const supplier = ref<ISupplier>({
  id: '',
  name: '',
  telephoneNr: '',
  email: '',
  addressId: '',
});

const suppliers = ref<ISupplier[]>([]);
const addresses = ref<IAddress[]>([]);

onMounted(async () => {
  suppliers.value = (await supplierService.getAllAsync()).data || [];
  addresses.value = (await addressService.getAllAsync()).data || [];
});

const doAdd = async () => {
  validationError.value = '';
  successMessage.value = '';

  try {
    const { id, ...rest } = supplier.value;
    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as ISupplier;

    const result = await supplierService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ Supplier has been successfully created!';
    }
  } catch (error) {
    console.error('Error creating supplier:', error);
  }
};

</script>

<template>
  <main class="supplier-wrapper">
    <div class="supplier-box">
      <h1 class="title">{{ $t('Create New') }} {{ $t('Supplier') }}</h1>

      <form @submit.prevent="doAdd">
        <label for="name">{{ $t('Name') }}</label>
        <input id="name" type="text" v-model="supplier.name" required />

        <label for="telephoneNr">{{ $t('Telephone nr') }}</label>
        <input id="telephoneNr" type="text" v-model="supplier.telephoneNr" required />

        <label for="email">{{ $t('Email') }}</label>
        <input id="email" type="text" v-model="supplier.email" required />

        <label for="address">{{ $t('Address') }}</label>
        <select id="address" v-model="supplier.addressId">
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Address') }} --</option>
          <option v-for="address in addresses" :key="address.id" :value="address.id">
            {{ address.name }}
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
.supplier-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 10vh;
  font-family: Arial, sans-serif;
}

.supplier-box {
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
