<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import type { ICurrentStockEnriched } from "@/domain/logic/ICurrentStockEnriched";
import type { ICurrentStock } from "@/domain/logic/ICurrentStock";
import type { IProduct } from "@/domain/logic/IProduct";
import { CurrentStockService } from "@/services/mvcServices/CurrentStockService";
import { ProductService } from "@/services/mvcServices/ProductServices";

const route = useRoute();
const storageRoomId = route.params.storageRoomId as string;

const service = new CurrentStockService();
const productService = new ProductService();

const data = ref<ICurrentStockEnriched[]>([]);
const products = ref<IProduct[]>([]);

const searchCode = ref("");
const searchName = ref("");
const showDrawer = ref(false);
const drawerMode = ref<'edit' | 'create'>('edit');

const activeEditStock = ref<ICurrentStock | null>(null);
const activeCreateStock = ref<ICurrentStock | null>(null);

const validationError = ref('');
const successMessage = ref('');

const emptyStock: ICurrentStock = {
  id: '',
  quantity: 0,
  updatedAt: '',
  productId: '',
  StorageRoomId: storageRoomId
};

onMounted(async () => {
  const result = await service.getByStorageRoomId(storageRoomId);
  data.value = result.data || [];
  products.value = (await productService.getAllAsync()).data || [];
});

const filteredCurrentStocks = computed(() =>
  data.value.filter((item) =>
    item.productName.toLowerCase().includes(searchName.value.toLowerCase()) &&
    item.productCode.toLowerCase().includes(searchCode.value.toLowerCase())
  )
);

const openEditDrawer = async (id: string) => {
  const all = await service.getAllAsync();
  const stock = all.data?.find((s) => s.id === id) || null;
  if (stock) {
    activeEditStock.value = { ...stock };
    drawerMode.value = 'edit';
    showDrawer.value = true;
  }
};

const openCreateDrawer = () => {
  activeCreateStock.value = { ...emptyStock };
  drawerMode.value = 'create';
  showDrawer.value = true;
};

const updateCurrentStock = async () => {
  if (!activeEditStock.value) return;
  validationError.value = '';
  successMessage.value = '';

  const result = await service.updateAsync(activeEditStock.value);
  if (result.errors?.length) {
    validationError.value = result.errors.join(', ');
  } else {
    successMessage.value = '✅ Stock updated!';
    showDrawer.value = false;
    const result = await service.getByStorageRoomId(storageRoomId);
    data.value = result.data || [];
  }
};

const createCurrentStock = async () => {
  if (!activeCreateStock.value) return;
  validationError.value = '';
  successMessage.value = '';

  const { id, ...rest } = activeCreateStock.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
  ) as unknown as ICurrentStock;

  const result = await service.addAsync(cleaned);
  if (result.errors?.length) {
    validationError.value = result.errors.join(', ');
  } else {
    successMessage.value = '✅ Stock created!';
    showDrawer.value = false;
    const result = await service.getByStorageRoomId(storageRoomId);
    data.value = result.data || [];
  }
};
</script>

<template>
  <main class="product-wrapper">
    <section class="product-header">
      <div class="product-header-left">
        <h1 class="page-title">🎬 Current Stock</h1>
        <p class="subtitle">Browse and manage your current inventory</p>
      </div>
    </section>

    <div class="filter-bar">
      <div class="filter-controls">
        <input v-model="searchCode" type="text" placeholder="Search by code" class="search-box" />
        <input v-model="searchName" type="text" placeholder="Search by name" class="search-box" />
      </div>
      <button class="create-link" @click="openCreateDrawer">+ Create New</button>
    </div>

    <div class="table-scroll-container">
      <div class="product-list">
        <div class="product-card" v-for="item in filteredCurrentStocks" :key="item.id">
          <div class="product-info">
            <h3>{{ item.productName }}</h3>
            <p><strong>Code:</strong> {{ item.productCode }}</p>
            <p><strong>Quantity:</strong> {{ item.quantity }}</p>
          </div>
          <div class="button-group">
            <button class="view-button" @click="openEditDrawer(item.id)">Edit</button>
            <router-link class="view-button" :to="`/product/${item.productId}`">View</router-link>
          </div>
        </div>
      </div>
    </div>

    <!-- Drawer -->
    <transition name="fade">
      <div v-if="showDrawer" class="drawer-overlay" @click.self="showDrawer = false">
        <div class="drawer">
          <h2>{{ drawerMode === 'edit' ? 'Edit Current Stock' : 'Create New Stock' }}</h2>

          <label>Quantity</label>
          <input v-if="drawerMode === 'edit'" v-model="activeEditStock!.quantity" type="number" />
          <input v-else v-model="activeCreateStock!.quantity" type="number" />

          <template v-if="drawerMode === 'create'">
            <label>Product</label>
            <select v-model="activeCreateStock!.productId">
              <option disabled value="">-- Select Product --</option>
              <option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option>
            </select>
          </template>

          <div class="drawer-buttons">
            <button v-if="drawerMode === 'edit'" @click="updateCurrentStock" class="update-btn">Update</button>
            <button v-else @click="createCurrentStock" class="update-btn">Create</button>
            <button @click="showDrawer = false" class="cancel-btn">Cancel</button>
          </div>

          <div class="text-danger" v-if="validationError">{{ validationError }}</div>
          <div class="text-success" v-if="successMessage">{{ successMessage }}</div>
        </div>
      </div>
    </transition>
  </main>
</template>

<style scoped>
.product-wrapper {
  height: 100%;
  display: flex;
  flex-direction: column;
  padding: 2rem;
  box-sizing: border-box;
  max-width: 1600px;
  margin: 0 auto;
  font-family: 'Inter', 'Segoe UI', sans-serif;
  color: white;
  background: transparent;
}

.product-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  flex-wrap: wrap;
}

.product-header-left {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.page-title {
  font-size: 2.6rem;
  font-weight: 800;
  color: #ffaa33;
  text-shadow: 0 0 10px rgba(255, 170, 51, 0.25);
  margin: 0;
}

.subtitle {
  font-size: 1rem;
  color: #bbb;
  margin: 0;
  opacity: 0.85;
}

.filter-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
  background: rgba(30, 30, 30, 0.6);
  backdrop-filter: blur(8px);
  padding: 1rem 1.5rem;
  border-radius: 16px;
  box-shadow: 0 0 12px rgba(255, 170, 51, 0.05);
}

.filter-controls {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.search-box {
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border-radius: 12px;
  border: 1px solid #ffaa33;
  background-color: rgba(43, 43, 43, 0.5);
  color: white;
  min-width: 220px;
  transition: all 0.2s ease;
}

.search-box:focus {
  outline: none;
  border-color: #ffc266;
  background-color: rgb(43, 43, 43);
}

.search-box::placeholder {
  color: #ccc;
}

.create-link {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  padding: 0.6rem 1.4rem;
  border-radius: 12px;
  font-weight: 700;
  text-decoration: none;
  box-shadow: 0 2px 6px rgba(255, 165, 0, 0.2);
  transition: all 0.2s ease;
}

.create-link:hover {
  background: linear-gradient(to right, #ffc56e, #ffa726);
  box-shadow: 0 3px 10px rgba(255, 165, 0, 0.3);
}

.table-scroll-container {
  flex-grow: 1;
  overflow-y: auto;
  padding: 1.2rem;
  margin-top: 1rem;
  border-radius: 16px;
  background: rgba(20, 20, 20, 0.5);
  backdrop-filter: blur(6px);
  box-shadow: inset 0 0 20px rgba(255, 165, 0, 0.05);
}

.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.product-card {
  background: rgba(45, 45, 45, 0.6);
  border-radius: 14px;
  padding: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.05);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.product-card:hover {
  box-shadow: 0 6px 16px rgba(255, 170, 51, 0.1);
  transform: translateY(-3px);
  border-color: #ffaa33;
}

.product-info h3 {
  color: #ffaa33;
  font-size: 1.1rem;
  margin-bottom: 0.4rem;
}

.product-info p {
  color: #ccc;
  font-size: 0.95rem;
  margin: 0.1rem 0;
}

.view-button {
  margin-top: 1rem;
  padding: 0.4rem 1rem;
  border: none;
  border-radius: 8px;
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-weight: bold;
  font-size: 0.9rem;
  cursor: pointer;
  transition: background 0.2s ease;
}

.view-button:hover {
  background-color: #ffc266;
}

.button-group {
  display: flex;
  gap: 0.5rem;
  margin-top: 0.5rem;
}

@media (max-width: 768px) {
  .product-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .filter-bar {
    flex-direction: column;
  }

  .product-list {
    grid-template-columns: 1fr;
  }
}

.drawer-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: flex-end;
  z-index: 999;
}

.drawer {
  width: 420px;
  background: linear-gradient(to bottom, #1e1e1e, #121212);
  color: white;
  padding: 2rem;
  overflow-y: auto;
  height: 100%;
  box-shadow: -8px 0 20px rgba(0, 0, 0, 0.5);
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
  border-left: 1px solid rgba(255, 255, 255, 0.05);
}

.drawer input {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  border: none;
  background: rgba(60, 60, 60, 0.7);
  color: white;
  font-size: 1rem;
  transition: all 0.2s;
}

.drawer input:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}

.drawer-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1.5rem;
}

.update-btn {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-size: 0.95rem;
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  font-weight: 700;
  border: none;
  cursor: pointer;
  box-shadow: 0 3px 8px rgba(0, 0, 0, 0.2);
}

.cancel-btn {
  background: #333;
  color: white;
  font-size: 0.95rem;
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  cursor: pointer;
  transition: all 0.2s ease;
}

.cancel-btn:hover {
  background: #444;
}

.text-danger {
  color: #ff4d4d;
  font-weight: bold;
  text-align: center;
}

.text-success {
  color: #28a745;
  font-weight: bold;
  text-align: center;
}

a.view-button {
  text-decoration: none;
}

.drawer select {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  border: none;
  background: rgba(60, 60, 60, 0.7);
  color: white;
  font-size: 1rem;
  appearance: none;
  background-image: url('data:image/svg+xml;charset=US-ASCII,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 4 5"><path fill="white" d="M2 0L0 2h4zm0 5L0 3h4z"/></svg>');
  background-repeat: no-repeat;
  background-position: right 0.75rem center;
  background-size: 0.65rem auto;
  transition: all 0.2s;
}

.drawer select:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}
</style>
