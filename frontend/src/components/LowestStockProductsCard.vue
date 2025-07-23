<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { CurrentStockService } from '@/services/mvcServices/CurrentStockService'

const service = new CurrentStockService()

interface ProductStock {
  productId: string
  productName: string
  quantity: number
}

const products = ref<ProductStock[]>([])

const fetchLowestStock = async () => {
  try {
    products.value = await service.getLowestStockProducts()
  } catch (error) {
    console.error('Failed to fetch low stock products:', error)
  }
}

onMounted(fetchLowestStock)
</script>

<template>
  <div class="lowest-stock-card">
    <h2>❗️ {{ $t('Low Stock Alert') }}</h2>
    <ul v-if="products.length">
      <li v-for="product in products" :key="product.productId" class="product-item">
        <span class="name">{{ product.productName }}</span>
        <span class="quantity">{{ product.quantity }} pcs</span>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('Stock data not available') }}</p>
  </div>
</template>

<style scoped>
.lowest-stock-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(255, 140, 0, 0.25);
  color: #fff;
  max-width: 350px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.lowest-stock-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #ffa500;
  margin: 0;
  border-bottom: 2px solid #ffa500;
  padding-bottom: 0.5rem;
}

ul {
  list-style: none;
  padding: 0;
  margin: 0;
  max-height: 200px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: #ffa500 #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}

ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}

ul::-webkit-scrollbar-thumb {
  background-color: #ffa500;
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

.quantity {
  flex: 0.8;
  text-align: right;
  color: #f0ad4e;
  font-weight: bold;
}

.empty {
  font-style: italic;
  color: #aaa;
  text-align: center;
}
</style>
