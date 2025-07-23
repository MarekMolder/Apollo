<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ProductService } from '@/services/mvcServices/ProductServices'
import type { IProductEnriched } from '@/domain/logic/IProductEnriched'

const productService = new ProductService()
const router = useRouter()

const allProducts = ref<IProductEnriched[]>([])
const query = ref('')
const filtered = ref<IProductEnriched[]>([])

onMounted(async () => {
  const result = await productService.getEnrichedProducts()
  allProducts.value = result.data ?? []
})

const filterResults = () => {
  const q = query.value.toLowerCase()
  filtered.value = allProducts.value.filter(p =>
    p.name.toLowerCase().includes(q) || p.code.toLowerCase().includes(q)
  )
}

const goToProduct = (id: string) => {
  router.push(`/product/${id}`)
}
</script>

<template>
  <div class="quick-search-card">
    <h2>🔍 {{ $t('Quick Product Search') }}</h2>
    <input
      v-model="query"
      type="text"
      :placeholder="$t('Search by name or code')"
      @input="filterResults"
    />
    <ul v-if="filtered.length">
      <li
        v-for="product in filtered.slice(0, 3)"
        :key="product.id"
        class="result-item"
      >
        <div class="info">
          <strong>{{ product.name }}</strong>
          <span class="code">({{ product.code }})</span>
        </div>
        <button @click="goToProduct(product.id)">View</button>
      </li>
    </ul>
    <p v-else class="empty">{{ $t('No match') }}</p>
  </div>
</template>

<style scoped>
.quick-search-card {
  background: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(255, 165, 0, 0.2);
  color: #fff;
  max-width: 400px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.quick-search-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: orange;
  margin: 0;
  border-bottom: 2px solid orange;
  padding-bottom: 0.5rem;
}

.quick-search-card input {
  width: 100%;
  padding: 0.7rem 1rem;
  border: 2px solid orange;
  border-radius: 0.6rem;
  background-color: #2c2c2c;
  color: white;
  font-size: 1rem;
  transition: border-color 0.3s ease;
}

.quick-search-card input:focus {
  outline: none;
  border-color: #ffcc66;
}

ul {
  list-style: none;
  margin: 0;
  padding: 0;
  max-height: 200px;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: orange #1e1e1e;
}

ul::-webkit-scrollbar {
  width: 6px;
}

ul::-webkit-scrollbar-track {
  background: #1e1e1e;
}

ul::-webkit-scrollbar-thumb {
  background-color: orange;
  border-radius: 4px;
}

.result-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.6rem 0;
  border-bottom: 1px solid #333;
  font-size: 0.95rem;
}

.result-item:last-child {
  border-bottom: none;
}

.result-item .info {
  display: flex;
  flex-direction: column;
}

.code {
  font-size: 0.85rem;
  color: #aaa;
}

button {
  background: orange;
  border: none;
  border-radius: 6px;
  padding: 0.5rem 0.8rem;
  cursor: pointer;
  font-weight: bold;
  color: black;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #ffb84d;
}

.empty {
  font-style: italic;
  color: #888;
  text-align: center;
}
</style>
