<script setup lang="ts">
import { onMounted, ref } from 'vue'
import axios from 'axios'
import {ActionService} from "@/services/mvcServices/ActionService.ts";


const service = new ActionService();

interface ProductStat {
  productId: string
  productName: string
  removeQuantity: number
}

const products = ref<ProductStat[]>([])

const fetchProblematicProducts = async () => {
  try {
    products.value = await service.getTopRemovedProducts()
  } catch (error) {
    console.error('Failed to fetch problematic products:', error)
  }
}

onMounted(fetchProblematicProducts)
</script>

<template>
  <div class="problematic-products-card">
    <h2>🚨 {{ $t('Most Removed Products') }}</h2>
    <ul v-if="products.length">
      <li v-for="product in products" :key="product.productId" class="product-item">
        <span class="name">{{ product.productName }}</span>
        <span class="count">{{ product.removeQuantity }}× {{ $t('Removed') }}</span>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('No problematic products found') }}</p>
  </div>
</template>

<style scoped>
.problematic-products-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(255, 0, 0, 0.25);
  color: #fff;
  max-width: 350px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.problematic-products-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #ff4d4d;
  margin: 0;
  border-bottom: 2px solid #ff4d4d;
  padding-bottom: 0.5rem;
}

ul {
  list-style: none;
  margin: 0;
  padding: 0;
  max-height: 200px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: #ff4d4d #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}

ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}

ul::-webkit-scrollbar-thumb {
  background-color: #ff4d4d;
  border-radius: 4px;
}

.product-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.4rem 0;
  border-bottom: 1px solid #333;
  font-size: 0.95rem;
}

.product-item:last-child {
  border-bottom: none;
}

.name {
  font-weight: 600;
  flex: 1;
}

.count {
  flex: 0.8;
  text-align: right;
  color: #ffb3b3;
}

.empty {
  font-style: italic;
  color: #aaa;
  text-align: center;
}
</style>
