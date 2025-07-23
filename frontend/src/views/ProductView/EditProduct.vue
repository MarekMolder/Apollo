<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { ProductService } from "@/services/mvcServices/ProductServices.ts";
import { ProductCategoryService } from "@/services/mvcServices/ProductCategoryService.ts";
import type { IProduct } from "@/domain/logic/IProduct.ts";
import type { IProductCategory } from "@/domain/logic/IProductCategory.ts";

const route = useRoute();
const productId = route.params.id as string;

const productService = new ProductService();
const productCategoryService = new ProductCategoryService();

const product = ref<IProduct | null>(null);
const productCategories = ref<IProductCategory[]>([]);
const successMessage = ref("");
const validationError = ref("");

onMounted(async () => {
  const allProducts = await productService.getAllAsync();
  product.value = allProducts.data?.find(p => p.id === productId) || null;

  const categories = await productCategoryService.getAllAsync();
  productCategories.value = categories.data || [];
});

const doUpdate = async () => {
  if (!product.value) return;
  successMessage.value = "";
  validationError.value = "";

  try {
    const result = await productService.updateAsync(product.value);
    if (result.errors?.length) {
      validationError.value = result.errors.join(", ");
    } else {
      successMessage.value = "✅ Product successfully updated!";
    }
  } catch (error) {
    console.error("Error updating product:", error);
  }
};
</script>

<template>
  <main class="product-wrapper">
    <div class="product-box" v-if="product">
      <h1 class="title">{{ $t('Edit') }} {{ $t('Product') }}</h1>
      <form @submit.prevent="doUpdate">
        <label for="unit">{{ $t('Unit') }}</label>
        <input id="unit" type="text" v-model="product.unit" required />

        <label for="volume">{{ $t('Volume') }}</label>
        <input id="volume" type="number" v-model="product.volume" required />

        <label for="code">{{ $t('Product Code') }}</label>
        <input id="code" type="text" v-model="product.code" required />

        <label for="name">{{ $t('Name') }}</label>
        <input id="name" type="text" v-model="product.name" required />

        <label for="price">{{ $t('Price') }}</label>
        <input id="price" type="number" step="0.01" v-model="product.price" required />

        <label for="productCategory">{{ $t('Product') }} {{ $t('Category') }}</label>
        <select id="productCategory" v-model="product.productCategoryId">
          <option disabled value="">-- {{ $t('Select') }} {{ $t('Category') }} --</option>
          <option v-for="category in productCategories" :key="category.id" :value="category.id">
            {{ category.name }}
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
