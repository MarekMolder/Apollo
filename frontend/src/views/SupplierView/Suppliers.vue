<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import { X } from "lucide-vue-next";
import type { ISupplier } from "@/domain/logic/ISupplier";
import type { ISupplierEnriched } from "@/domain/logic/ISupplierEnriched";
import { SupplierService } from "@/services/mvcServices/SupplierService";
import {AddressService} from "@/services/mvcServices/AddressService.ts";
import type {IAddress} from "@/domain/logic/IAddress.ts";
import suppliers from "@/views/SupplierView/Suppliers.vue";
import type {IProductEnriched} from "@/domain/logic/IProductEnriched.ts";
import {ProductService} from "@/services/mvcServices/ProductServices.ts";
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();

// Services
const supplierService = new SupplierService();
const addressService = new AddressService();

// Entity's
const supplierAddresses = ref<IAddress[]>([]);
const data = ref<ISupplierEnriched[]>([]);
const productsForSupplier = ref<IProductEnriched[]>([]);

// Search engine
const searchName = ref("");

// Drawer mode
const showDrawer = ref(false);
const showProductsDrawer = ref(false);
const drawerMode = ref<"edit" | "create">("edit");
const activeEditSupplier = ref<ISupplierEnriched | null>(null);
const activeCreateSupplier = ref<ISupplier | null>(null);

//??
const selectedSupplierName = ref("");

// Messages errors/success
const validationError = ref('');
const successMessage = ref('');

// Empty Supplier entity
const emptySupplier = ref<ISupplier>({
  id: "",
  name: "",
  telephoneNr: "",
  email: "",
  addressId: ""
});

// Get suppliers
onMounted(async () => {
  suppliers.value = (await supplierService.getAllAsync()).data || [];
  supplierAddresses.value = (await addressService.getAllAsync()).data || [];
});

const fetchPageData = async () => {
  try {
    const result = await supplierService.getEnrichedSuppliers();
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching suppliers:", error);
  }
};

// Get products by Supplier
const fetchProductsBySupplier = async (supplierId: string, supplierName: string) => {
  try {
    selectedSupplierName.value = supplierName;
    const productService = new ProductService();
    const result = await productService.getBySupplier(supplierId);
    productsForSupplier.value = result.data || [];
    showProductsDrawer.value = true;
  } catch (error) {
    console.error("Error fetching products by supplier:", error);
  }
};

onMounted(fetchPageData);

// Search engine filtered suppliers
const filteredSuppliers = computed(() =>
  data.value.filter((supplier) =>
    supplier.name.toLowerCase().includes(searchName.value.toLowerCase())
  )
);

// Drawers for Suppliers
const openSupplierEditDrawer = (supplier: ISupplierEnriched) => {
  activeEditSupplier.value = { ...supplier };
  drawerMode.value = "edit";
  showDrawer.value = true;
};

const openSupplierCreateDrawer = () => {
  activeCreateSupplier.value = emptySupplier.value;
  drawerMode.value = "create";
  showDrawer.value = true;
};

const activeSupplier = computed({
  get() {
    return drawerMode.value === "edit" ? activeEditSupplier.value : activeCreateSupplier.value;
  },
  set(value) {
    if (drawerMode.value === "edit") activeEditSupplier.value = value as ISupplierEnriched;
    else activeCreateSupplier.value = value as ISupplier;
  }
});

// Supplier edit function
const editSupplier = async () => {
  if (!activeEditSupplier.value) return;
  await supplierService.updateAsync(activeEditSupplier.value);
  showDrawer.value = false;
  await fetchPageData();
};

// Supplier create function
const createSupplier = async () => {
  validationError.value = '';
  successMessage.value = '';

  try {
    if (!activeCreateSupplier.value) return;

    const { id, ...rest } = activeCreateSupplier.value;

    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as ISupplier;

    const result = await supplierService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ Supplier has been successfully created!';
      showDrawer.value = false;
      await fetchPageData();
    }
  } catch (error) {
    console.error('Error creating supplier:', error);
  }
};

//Supplier remove function
const removeSupplier = async (id: string) => {
  if (!confirm("Are you sure you want to delete this supplier?")) return;
  await supplierService.removeAsync(id);
  await fetchPageData();
};

const selectedAddress = computed({
  get: () =>
    supplierAddresses.value.find(a => a.id === (activeSupplier.value?.addressId ?? '')) || null,
  set: (val: IAddress | null) => {
    if (activeSupplier.value) activeSupplier.value.addressId = val?.id ?? '';
  }
});
</script>

<template>
  <main
    :class="[
    'transition-all duration-300 p-4 sm:p-6 lg:p-8 text-white max-w-screen-2xl',
    sidebarStore.isOpen ? 'ml-[160px]' : 'ml-[64px]'
  ]"
  >
    <section class="mb-8 text-center">
      <h1
        class="text-4xl sm:text-5xl font-[Playfair_Display] font-bold tracking-[0.02em]
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)] relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          Suppliers
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">Browse and manage all supplier data</p>
    </section>

    <!-- Card container -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8 bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
               border-1 border-neutral-700 shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]">

        <!-- Filters in one row -->
        <div
          class="mb-6 flex items-center gap-3 whitespace-nowrap overflow-x-auto
                 [-ms-overflow-style:none] [scrollbar-width:none]">
          <div class="relative shrink-0">
            <input
              v-model="searchName"
              type="text"
              placeholder="Search by name"
              class="w-[300px] appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
                     h-11 text-sm pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30
                     focus:border-neutral-500 transition shadow-inner shadow-black/30"
            />
            <i class="bi bi-search pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
          </div>

          <div class="shrink-0 ml-1">
            <button
              @click="openSupplierCreateDrawer"
              class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                     border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                     shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_8px_24px_rgba(0,0,0,0.35)]
                     hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                     focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
              <i class="bi bi-plus-lg opacity-90 group-hover:opacity-100"></i>
              <span>New Supplier</span>
            </button>
          </div>
        </div>

        <!-- Cards grid -->
        <div class="grid gap-4 grid-cols-[repeat(auto-fill,minmax(260px,1fr))]">
          <div
            v-for="item in filteredSuppliers"
            :key="item.id"
            class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
                   shadow-[0_2px_8px_rgba(0,0,0,0.25)] hover:bg-white/10 focus:outline-none
                   focus:ring-2 focus:ring-white/15 transition">

            <!-- Remove -->
            <button
              class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                     border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                     hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
              @click="removeSupplier(item.id)"
              title="Remove supplier">
              <i class="bi bi-trash3"></i>
            </button>

            <h3 class="text-lg font-semibold text-neutral-100 pr-10">{{ item.name }}</h3>
            <p class="text-sm text-neutral-400 mt-1"><span class="text-neutral-300 font-medium">Phone:</span> {{ item.telephoneNr }}</p>
            <p class="text-sm text-neutral-400"><span class="text-neutral-300 font-medium">Email:</span> {{ item.email }}</p>
            <p class="text-sm text-neutral-400"><span class="text-neutral-300 font-medium">Address:</span> {{ item.fullAddress }}</p>

            <div class="mt-4 flex items-center justify-end gap-2">
              <button
                class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                       border-1 border-neutral-700 bg-white/5 text-neutral-200
                       hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                @click="fetchProductsBySupplier(item.id, item.name)">
                <i class="bi bi-bag-check"></i>
                Products
              </button>
              <button
                class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                       border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                       shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_3px_10px_rgba(0,0,0,0.35)]
                       hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition"
                @click="openSupplierEditDrawer(item)">
                <i class="bi bi-pencil"></i>
                Edit
              </button>
            </div>
          </div>
        </div>

        <!-- Empty state -->
        <div v-if="filteredSuppliers.length === 0" class="text-center text-neutral-400 mt-8">
          No suppliers found.
        </div>
      </div>
    </section>

    <!-- CENTERED MODAL: Create/Edit Supplier -->
    <transition name="fade">
      <div v-if="showDrawer" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="showDrawer = false">
        <div
          class="w-full max-w-xl rounded-2xl border border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
                 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ drawerMode === 'edit' ? (activeEditSupplier?.name || 'Edit Supplier') : 'Create New Supplier' }}
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                     border border-white/10 bg-white/5 text-neutral-300
                     hover:bg-white/10 hover:text-white focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="showDrawer = false" title="Close">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <!-- Form -->
          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
              <input v-model="activeSupplier!.name" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Phone</label>
              <input v-model="activeSupplier!.telephoneNr" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Email</label>
              <input v-model="activeSupplier!.email" type="email"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Address</label>
              <div class="relative">
                <Multiselect
                  v-model="selectedAddress"
                  :options="supplierAddresses"
                  track-by="id"
                  :custom-label="a => a.name"
                  :searchable="true"
                  :close-on-select="true"
                  :allow-empty="false"
                  placeholder="-- Select Address --"
                  class="multiselect-dark"
                />
                <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
              </div>
            </div>
          </div>

          <!-- Messages -->
          <div class="mt-4">
            <p v-if="validationError" class="text-rose-400 text-center font-medium">{{ validationError }}</p>
            <p v-if="successMessage" class="text-emerald-400 text-center font-medium">{{ successMessage }}</p>
          </div>

          <!-- Actions -->
          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button
              v-if="drawerMode === 'edit'"
              @click="editSupplier"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Update
            </button>
            <button
              v-else
              @click="createSupplier"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Create
            </button>
            <button
              @click="showDrawer = false"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- CENTERED MODAL: Products by Supplier -->
    <transition name="fade">
      <div v-if="showProductsDrawer" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="showProductsDrawer = false">
        <div
          class="w-full max-w-2xl rounded-2xl border border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
                 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              Products by {{ selectedSupplierName }}
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                     border-1 border-neutral-700 bg-white/5 text-neutral-300
                     hover:bg-white/10 hover:text-white focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="showProductsDrawer = false" title="Close">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-4 max-h-[420px] overflow-y-auto space-y-3 pr-1">
            <div v-if="productsForSupplier.length === 0" class="text-neutral-400">
              No products found for this supplier.
            </div>
            <div v-for="product in productsForSupplier" :key="product.id"
                 class="rounded-lg border border-white/10 bg-white/5 p-4">
              <div class="font-medium text-neutral-100">{{ product.name }}</div>
              <div class="text-sm text-neutral-400 mt-1">
                Code: {{ product.code }} • Price: {{ product.price }} • Qty: {{ product.quantity }} {{ product.unit }}
              </div>
            </div>
          </div>

          <div class="mt-6 flex justify-end">
            <button
              @click="showProductsDrawer = false"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Close
            </button>
          </div>
        </div>
      </div>
    </transition>
  </main>
</template>

<style scoped>
/* --- vue-multiselect: tume variant --- */
:deep(.multiselect-dark) {
  /* konteiner */
  border: 1px solid rgba(255,255,255,0.1);
  border-radius: 0.75rem;
  background: rgba(17,17,17,0.7);
  color: #fff;
}
:deep(.multiselect-dark .multiselect__tags) {
  min-height: 44px;
  padding: 0 12px;
  background: transparent;
  border: 0;
  display: flex;
  align-items: center;
}
:deep(.multiselect-dark .multiselect__placeholder),
:deep(.multiselect-dark .multiselect__single) {
  line-height: 44px;
  padding: 0;
  margin: 0;
  background: transparent;
  color: #d1d5db; /* neutral-300 */
}
:deep(.multiselect-dark .multiselect__input) {
  background: transparent;
  color: #fff;
  line-height: 44px;
  padding: 0;
  margin: 0;
}
:deep(.multiselect-dark .multiselect__select),
:deep(.multiselect-dark .multiselect__clear) {
  color: #9ca3af; /* neutral-400 */
}
:deep(.multiselect-dark.multiselect--active .multiselect__tags) {
  outline: none;
  box-shadow: 0 0 0 2px rgba(34,211,238,0.35);
  border-color: rgba(34,211,238,0.4);
}
:deep(.multiselect-dark .multiselect__content-wrapper) {
  margin-top: 8px;
  background: rgba(10,10,10,0.95);
  backdrop-filter: blur(6px);
  border: 1px solid rgba(255,255,255,0.1);
  border-radius: 0.75rem;
  max-height: 16rem;
}
:deep(.multiselect-dark .multiselect__option) {
  padding: 8px 12px;
  color: #e5e7eb; /* neutral-200 */
}
:deep(.multiselect-dark .multiselect__option--highlight) {
  background: rgba(255,255,255,0.1);
}
:deep(.multiselect-dark .multiselect__option--selected) {
  background: rgba(255,255,255,0.06);
  color: #ffaa33;
}
:deep(.multiselect-dark.multiselect--disabled) {
  opacity: .6;
  cursor: not-allowed;
}

</style>
