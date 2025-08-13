<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import type { IProductEnriched } from '@/domain/logic/IProductEnriched';
import { ProductService } from '@/services/mvcServices/ProductServices';

// Services
const service = new ProductService();

//Entity's
const product = ref<IProductEnriched | null>(null);

// Route
const route = useRoute();
const productId = route.params.id as string;

// Get products
const fetchProduct = async () => {
  try {
    const result = await service.getEnrichedProducts();
    product.value = (result.data || []).find(p => p.id === productId) || null;
  } catch (error) {
    console.error('Error fetching product:', error);
  }
};

onMounted(fetchProduct);
</script>

<template>
  <div class="product-detail-wrapper" v-if="product">

    <div class="product-detail-card">
      <h1>{{ product.name }}</h1>
      <div class="image-placeholder">🖼️</div>
      <div class="info">
        <p><strong>{{ $t('Product Code') }}:</strong> {{ product.code }}</p>
        <p><strong>{{ $t('Unit') }}:</strong> {{ product.unit }}</p>
        <p><strong>{{ $t('Volume') }}:</strong> {{ product.volume }}</p>
        <p><strong>{{ $t('Price') }}:</strong> €{{ product.price }}</p>
        <p><strong>{{ $t('Quantity') }}:</strong> {{ product.quantity }}</p>
        <p><strong>{{ $t('Category') }}:</strong> {{ product.productCategoryName }}</p>
      </div>
    </div>
  </div>

  <div v-else class="loading">
    Loading product info...
  </div>
</template>

<style scoped>
.product-detail-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  font-family: "Segoe UI", sans-serif;
  color: white;
}

.product-detail-card {
  background-color: #2a2a2a;
  border: 3px solid orange;
  border-radius: 16px;
  box-shadow: 0 0 12px rgba(255, 165, 0, 0.3);
  padding: 2rem;
  width: 100%;
  max-width: 600px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.5rem;
}

.image-placeholder {
  width: 100px;
  height: 100px;
  background-color: orange;
  border-radius: 50%;
  font-size: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.info {
  background-color: #fff;
  color: #000;
  padding: 1rem 1.5rem;
  border-radius: 12px;
  width: 100%;
  box-shadow: 0 0 8px rgba(0, 0, 0, 0.1);
}

.info p {
  margin: 0.5rem 0;
  padding-bottom: 0.3rem;
  border-bottom: 1px solid #ddd;
}

.info p:last-child {
  border-bottom: none;
}

.loading {
  color: orange;
  font-size: 1.2rem;
  padding: 2rem;
}

.header-row {
  display: flex;
  justify-content: flex-end;
  width: 100%;
}

.edit-link {
  background-color: orange;
  padding: 0.4rem 1rem;
  color: white;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  font-size: 0.95rem;
  cursor: pointer;
  transition: background 0.3s ease;
}

.edit-link:hover {
  background-color: #ffaa33;
}

.remove-link {
  background-color: #dc3545;
  padding: 0.4rem 1rem;
  color: white;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  font-size: 0.95rem;
  cursor: pointer;
  margin-left: 1rem;
  transition: background 0.3s ease;
}

.remove-link:hover {
  background-color: #e55353;
}
</style>
