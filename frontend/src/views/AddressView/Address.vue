<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import { X } from "lucide-vue-next";
import {AddressService} from "@/services/mvcServices/AddressService.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";
import type {IAddressEnriched} from "@/domain/logic/IAddressEnriched.ts";

// Services
const addressService = new AddressService();

// Entity's
const data = ref<IAddressEnriched[]>([]);

// Search engine
const searchName = ref("");

// Drawer mode
const showDrawer = ref(false);
const drawerMode = ref<"edit" | "create">("edit");
const activeEditAddress = ref<IAddress | null>(null);
const activeCreateAddress = ref<IAddress | null>(null);

// Messages errors/success
const validationError = ref('');
const successMessage = ref('');

// Empty Address entity
const emptyAddress = ref<IAddress>({
  id: '',
  streetName: '',
  buildingNr: 0,
  city: '',
  province: '',
  country: '',
  name: '',
  postalCode: '',
  unitNr: null,
});

// Get Addresses
onMounted(async () => {
  data.value = (await addressService.getEnrichedAddresses()).data || [];
});

const fetchPageData = async () => {
  try {
    const result = await addressService.getEnrichedAddresses();
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching addresses:", error);
  }
};

onMounted(fetchPageData);

// Search engine filtered Address
const filteredAddress = computed(() =>
  data.value.filter((address) =>
    address.name.toLowerCase().includes(searchName.value.toLowerCase())
  )
);

// Drawers for Address
const openAddressEditDrawer = (address: IAddress) => {
  activeEditAddress.value = { ...address };
  drawerMode.value = "edit";
  showDrawer.value = true;
};

const openAddressCreateDrawer = () => {
  activeCreateAddress.value = emptyAddress.value;
  drawerMode.value = "create";
  showDrawer.value = true;
};

const activeAddress = computed({
  get() {
    return drawerMode.value === "edit" ? activeEditAddress.value : activeCreateAddress.value;
  },
  set(value) {
    if (drawerMode.value === "edit") activeEditAddress.value = value as IAddress;
    else activeCreateAddress.value = value as IAddress;
  }
});

// Address edit function
const editAddress = async () => {
  if (!activeEditAddress.value) return;
  await addressService.updateAsync(activeEditAddress.value);
  showDrawer.value = false;
  await fetchPageData();
};

// Address create function
const createAddress = async () => {
  validationError.value = '';
  successMessage.value = '';

  try {
    if (!activeCreateAddress.value) return;

    const { id, ...rest } = activeCreateAddress.value;

    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as IAddress;

    const result = await addressService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ Address has been successfully created!';
      showDrawer.value = false;
      await fetchPageData();
    }
  } catch (error) {
    console.error('Error creating address:', error);
  }
};

//Address remove function
const removeReason = async (id: string) => {
  if (!confirm("Are you sure you want to delete this address?")) return;
  await addressService.removeAsync(id);
  await fetchPageData();
};
</script>

<template>
  <div v-if="validationError" class="text-danger">{{ validationError }}</div>
  <div v-if="successMessage" class="text-success">{{ successMessage }}</div>
  <main class="product-wrapper">
    <section class="product-header">
      <div class="product-header-left">
        <h1 class="page-title">Addresses</h1>
        <p class="subtitle">Browse and manage all Addresses data</p>
      </div>
    </section>

    <div class="filter-bar">
      <div class="filter-controls">
        <input
          v-model="searchName"
          type="text"
          placeholder="Search by name"
          class="search-box"
        />
      </div>
      <button class="create-link" @click="openAddressCreateDrawer">+ Create New</button>
    </div>

    <div class="table-scroll-container">
      <div class="product-list">
        <div class="product-card" v-for="item in filteredAddress" :key="item.id">
          <div class="product-info">
            <button class="remove-icon-btn" @click="removeReason(item.id)" title="Remove supplier">
              <X />
            </button>
            <h3>{{ item.name }}</h3>
            <p><strong>StreetName:</strong> {{ item.streetName }}</p>
            <p><strong>Building nr:</strong> {{ item.buildingNr }}</p>
            <p><strong>City:</strong> {{ item.city }}</p>
            <p><strong>Province:</strong> {{ item.province }}</p>
            <p><strong>Country:</strong> {{ item.country }}</p>
            <p><strong>Unit nr:</strong> {{ item.unitNr }}</p>

            <p><strong>Full Address:</strong> {{ item.fullAddress }}</p>
          </div>
          <button class="view-button" @click="openAddressEditDrawer(item)">View</button>
        </div>
      </div>
    </div>

    <!-- DRAWER MODAL -->
    <transition name="fade">
      <div v-if="showDrawer" class="drawer-overlay" @click.self="showDrawer = false">
        <div class="drawer">
          <h2>{{
              drawerMode === "edit" ? activeEditAddress?.name : "Create New Address"
            }}</h2>

          <label>Name</label>
          <input v-model="activeAddress!.name" type="text" />

          <label>StreetName</label>
          <input v-model="activeAddress!.streetName" type="text" />

          <label>Building nr</label>
          <input v-model="activeAddress!.buildingNr" type="number" />

          <label>Unit nr</label>
          <input v-model="activeAddress!.unitNr" type="number" />

          <label>Postal code</label>
          <input v-model="activeAddress!.postalCode" type="text" />

          <label>City</label>
          <input v-model="activeAddress!.city" type="text" />

          <label>Province</label>
          <input v-model="activeAddress!.province" type="text" />

          <label>Country</label>
          <input v-model="activeAddress!.country" type="text" />

          <div class="drawer-buttons">
            <button v-if="drawerMode === 'edit'" @click="editAddress" class="update-btn">Update</button>
            <button v-else @click="createAddress" class="update-btn">Create</button>
            <button @click="showDrawer = false" class="cancel-btn">Cancel</button>
          </div>
        </div>
      </div>
    </transition>
  </main>
</template>

<style scoped>
html,
body {
  height: 100%;
  margin: 0;
  overflow: hidden; /* oluline! leht ei scrolli */
}

/* --- Layout --- */
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

/* --- Filter Bar --- */
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

.filter-bar select,
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

.filter-bar select:focus {
  outline: none;
  border-color: #ffc266;
  background-color: rgb(43, 43, 43);
}

.filter-bar input::placeholder {
  color: #ccc;
}

/* --- Create Button --- */
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

/* --- Table/Product Cards --- */
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
  position: relative;
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
  align-self: flex-end;
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

/* --- Drawer Overlay --- */
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
  border: 1px solid rgba(255,255,255,0.1);
  cursor: pointer;
  transition: all 0.2s ease;
}

.cancel-btn:hover {
  background: #444;
}

/* --- Responsive --- */
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

.drawer-select {
  width: 100%;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  background: rgba(60, 60, 60, 0.7);
  color: white;
  font-size: 1rem;
  border: none;
  appearance: none;
  transition: all 0.2s;
}

.drawer-select:focus {
  outline: none;
  background: rgba(80, 80, 80, 0.85);
  border: 1px solid #ffaa33;
}

.drawer-select option {
  background-color: #1e1e1e;
  color: white;
}

.remove-icon-btn {
  position: absolute;
  top: 0.6rem;
  right: 0.6rem;
  background: rgba(255, 80, 80, 0.15);
  border: 1px solid rgba(255, 80, 80, 0.6);
  border-radius: 50%;
  width: 28px;
  height: 28px;
  color: #ff5f5f;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  justify-content: center;
  backdrop-filter: blur(2px);
  box-shadow: 0 2px 8px rgba(255, 0, 0, 0.1);
  transition: all 0.2s ease;
}

.remove-icon-btn:hover {
  background: rgba(255, 50, 50, 0.3);
  border-color: rgba(255, 80, 80, 1);
  color: white;
}
</style>


