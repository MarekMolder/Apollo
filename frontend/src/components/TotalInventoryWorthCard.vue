<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { CurrentStockService } from '@/services/mvcServices/CurrentStockService'
import { InventoryService } from '@/services/mvcServices/InventoryService'
import type {IInventory} from "@/domain/logic/IInventory.ts";

const currentStockService = new CurrentStockService()
const inventoryService = new InventoryService()

const inventories = ref<{ id: string; name: string }[]>([])
const selectedInventory = ref<string | ''>('')
const total = ref<number | null>(null)

const fetchInventories = async () => {
  const result = await inventoryService.getAllAsync()
  if (result.data) {
    inventories.value = result.data.map((i: IInventory) => ({
      id: i.id,
      name: i.name
    }))
  } else {
    inventories.value = []
  }
}

const fetchTotalWorth = async () => {
  if (!selectedInventory.value) {
    total.value = null
    return
  }
  total.value = await currentStockService.getTotalWorth(selectedInventory.value)
}

onMounted(fetchInventories)
</script>

<template>
  <div class="inventory-worth-card">
    <h2>💼 {{ $t('Select Inventory') }}</h2>
    <select v-model="selectedInventory" @change="fetchTotalWorth">
      <option disabled value="">{{ $t('Choose') }}</option>
      <option v-for="inv in inventories" :key="inv.id" :value="inv.id">
        {{ inv.name }}
      </option>
    </select>

    <div v-if="total !== null" class="amount">
      {{ $t('Total Worth') }}: <strong>€{{ total.toFixed(2) }}</strong>
    </div>
    <div v-else class="empty">{{ $t('Select an inventory to see total worth') }}</div>
  </div>
</template>

<style scoped>
.inventory-worth-card {
  background-color: #1e1e1e;
  padding: 1.5rem;
  border-radius: 1rem;
  color: white;
  box-shadow: 0 6px 20px rgba(255, 165, 0, 0.2);
  max-width: 400px;
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.inventory-worth-card h2 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #ffa500;
  margin: 0;
  border-bottom: 2px solid #ffa500;
  padding-bottom: 0.5rem;
}

select {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 0.6rem;
  background-color: #2a2a2a;
  color: #fff;
  border: 2px solid #ffa500;
  font-size: 1rem;
  transition: border-color 0.3s ease;
}

select:focus {
  outline: none;
  border-color: #ffcc66;
}

.amount {
  font-size: 1.2rem;
  color: #ffcc66;
  font-weight: bold;
  text-align: center;
}

.empty {
  font-style: italic;
  color: #aaa;
  text-align: center;
}
</style>
