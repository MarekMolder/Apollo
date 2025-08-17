<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import type { IProductEnriched } from "@/domain/logic/IProductEnriched";
import { ProductService } from "@/services/mvcServices/ProductServices";
import type { IProduct } from "@/domain/logic/IProduct.ts";
import {ProductCategoryService} from "@/services/mvcServices/ProductCategoryService.ts";
import type {IProductCategory} from "@/domain/logic/IProductCategory.ts";
import products from "@/views/ProductView/Products.vue";
import Multiselect from 'vue-multiselect'
import 'vue-multiselect/dist/vue-multiselect.min.css'
import type {ISupplier} from "@/domain/logic/ISupplier.ts";
import {SupplierService} from "@/services/mvcServices/SupplierService.ts";
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();

// Services
const service = new ProductService();
const productCategoryService = new ProductCategoryService();
const supplierService = new SupplierService();

// Entity's
const data = ref<IProductEnriched[]>([]);
const productCategories = ref<IProductCategory[]>([]);
const productSuppliers = ref<ISupplier[]>([]);

// Search engine
const selectedCategory = ref("All");
const selectedSupplier = ref("All");
const searchCode = ref("");
const searchName = ref("");
const categories = ref<string[]>([]);
const suppliers = ref<string[]>([]);

// Drawer mode
const showDrawer = ref(false);
const drawerMode = ref<'edit' | 'create'>('edit');
const activeEditProduct = ref<IProductEnriched | null>(null);
const activeCreateProduct = ref<IProduct  | null>(null);


// Messages errors/success
const validationError = ref('');
const successMessage = ref('');

// Empty Product entity
const emptyProduct = ref<IProduct>({
  id: '',
  unit: '',
  volume: 0,
  volumeUnit: '',
  code: '',
  name: '',
  price: 0,
  quantity: 1,
  productCategoryId: '',
  isComponent: false,
  supplierId: '',
});

// Get products
onMounted(async () => {
  products.value = (await service.getEnrichedProducts()).data || [];
  productCategories.value = (await productCategoryService.getAllAsync()).data || [];
  productSuppliers.value = (await supplierService.getAllAsync()).data || [];
});

const fetchPageData = async () => {
  try {
    const result = await service.getEnrichedProducts();
    data.value = result.data || [];

    console.log("Loaded products:", data.value);

    categories.value = [
      "All",
      ...new Set(data.value.map((item) => item.productCategoryName))
    ];
    suppliers.value = [
      "All",
      ...new Set(data.value.map((item) => item.supplierName))
    ];
  } catch (error) {
    console.error("Error fetching products:", error);
  }
};

onMounted(fetchPageData);

// Search engine filtered suppliers
const filteredProducts = computed(() =>
  data.value.filter((product) => {
    const matchCategory =
      selectedCategory.value === "All" ||
      String(product.productCategoryId) === String(selectedCategory.value);

    const matchSupplier =
      selectedSupplier.value === "All" ||
      String(product.supplierId) === String(selectedSupplier.value);

    const matchSearchCode =
      product.code.toLowerCase().includes(searchCode.value.toLowerCase());
    const matchSearchName =
      product.name.toLowerCase().includes(searchName.value.toLowerCase());

    return matchCategory && matchSupplier && matchSearchCode && matchSearchName;
  })
);


// Drawers for Products
const openProductDrawer = (product: IProductEnriched) => {
  activeEditProduct.value = { ...product };
  selectedCategory.value = product.productCategoryId ?? 'All';
  selectedSupplier.value = product.supplierId ?? 'All';
  drawerMode.value = 'edit';
  showDrawer.value = true;
};

const openCreateDrawer = () => {
  activeCreateProduct.value = JSON.parse(JSON.stringify(emptyProduct.value));
  selectedCategory.value = 'All';
  selectedSupplier.value = 'All';
  drawerMode.value = 'create';
  showDrawer.value = true;
};

const activeProduct = computed({
  get() {
    return drawerMode.value === 'edit' ? activeEditProduct.value : activeCreateProduct.value;
  },
  set(value) {
    if (drawerMode.value === 'edit') activeEditProduct.value = value as IProductEnriched;
    else activeCreateProduct.value = value as IProduct;
  }
});

// Product edit function
const editProduct = async () => {
  if (!activeEditProduct.value) return;
  try {
    await service.updateAsync(activeEditProduct.value);
    showDrawer.value = false;
    await fetchPageData();
  } catch (err) {
    console.error("Failed to update product", err);
  }
};

// Product create function
const createProduct = async () => {
  validationError.value = '';
  successMessage.value = '';

  try {
    if (!activeCreateProduct.value) return;

    const { id, ...rest } = activeCreateProduct.value;

    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== '')
    ) as unknown as IProduct;
    console.log(cleaned);

    const result = await service.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(', ');
    } else {
      successMessage.value = '✅ Product has been successfully created!';
      showDrawer.value = false;
      await fetchPageData();
    }
  } catch (error) {
    console.error('Error creating product:', error);
  }
};

//Product remove function
const removeProduct = async (id: string) => {
  if (!confirm("Are you sure you want to delete this product?")) return;

  try {
    await service.removeAsync(id);
    await fetchPageData();
  } catch (error) {
    console.error("Error deleting product:", error);
    alert("Failed to delete product.");
  }
};

const selectedSupplierObj = computed({
  get() {
    if (selectedSupplier.value === 'All') return { id: 'All', name: 'All Suppliers' } as ISupplier
    return productSuppliers.value.find(s => s.id === selectedSupplier.value) || null
  },
  set(val: ISupplier | null) {
    selectedSupplier.value = val?.id ?? 'All'
    if (drawerMode.value === 'create' && activeCreateProduct.value) {
      activeCreateProduct.value.supplierId = val?.id ?? '';
    }
    if (drawerMode.value === 'edit' && activeEditProduct.value) {
      activeEditProduct.value.supplierId = val?.id ?? '';
    }
  }
})

const selectedCategoryObj = computed({
  get() {
    if (selectedCategory.value === 'All') return { id: 'All', name: 'All Categories' } as IProductCategory
    return productCategories.value.find(c => c.id === selectedCategory.value) || null;
  },
  set(val: IProductCategory | null) {
    selectedCategory.value = val?.id ?? 'All';
    if (drawerMode.value === 'create' && activeCreateProduct.value) {
      activeCreateProduct.value.productCategoryId = val?.id ?? '';
    }
    if (drawerMode.value === 'edit' && activeEditProduct.value) {
      activeEditProduct.value.productCategoryId = val?.id ?? '';
    }
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
               drop-shadow-[0_2px_12px_rgba(255,255,255,0.06)]
               relative inline-block">
        <span class="bg-gradient-to-b from-neutral-50 via-neutral-300 to-neutral-200 bg-clip-text text-transparent">
          Products
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">Browse and manage all available items</p>
    </section>

    <!-- Filter bar / toolbar -->
    <section class="mx-auto w-full max-w-[100rem]">
      <div
        class="rounded-[16px] p-6 sm:p-8
         bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
         border-1 border-neutral-700
         shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]
         max-w-[95%] mx-auto"
      >

        <!-- Üks rida, kitsad väljad, võimalik horisontaalne scroll -->
        <!-- Filter bar + nupp ühel real -->
        <div
          class="mb-6 flex flex-wrap gap-3 items-center justify-between
         overflow-x-auto px-2"
        >
          <!-- vasak blokk: filtrid -->
          <div class="flex flex-wrap gap-3">
            <!-- Category -->
            <div class="relative w-48">
              <label class="sr-only">Category</label>
              <select v-model="selectedCategory"
                      class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30 focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9">
                <option value="All">All Categories</option>
                <option v-for="c in productCategories" :key="c.id" :value="c.id">{{ c.name }}</option>
              </select>
              <i class="bi bi-list-ul absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
              <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>

            <!-- Supplier -->
            <div class="relative w-56">
              <label class="sr-only">Supplier</label>
              <select
                v-model="selectedSupplier"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
               px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30
               focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9"
              >
                <option value="All">All Suppliers</option>
                <option v-for="s in productSuppliers" :key="s.id" :value="s.id">{{ s.name }}</option>
              </select>
              <i class="bi bi-truck absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
              <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>

            <!-- Search by code -->
            <div class="relative w-48">
              <input
                v-model="searchCode"
                type="text"
                placeholder="Search by code"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
               h-11 text-medium pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30
               focus:border-neutral-500 transition shadow-inner shadow-black/30"
              />
              <i class="bi bi-upc-scan pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>

            <!-- Search by name -->
            <div class="relative w-48">
              <input
                v-model="searchName"
                type="text"
                placeholder="Search by name"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
               h-11 text-medium pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30
               focus:border-neutral-500 transition shadow-inner shadow-black/30"
              />
              <i class="bi bi-search pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>
          </div>

          <!-- parem blokk: nupp -->
          <div class="flex-shrink-0">
            <button
              @click="openCreateDrawer"
              class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
             border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
             shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
             hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
             focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition"
            >
              <i class="bi bi-plus-lg opacity-90 group-hover:opacity-100"></i>
              <span>New Product</span>
            </button>
          </div>
        </div>

        <!-- Cards grid -->
        <div class="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 2xl:grid-cols-5">
          <div
            v-for="item in filteredProducts"
            :key="item.id"
            class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
           shadow-[0_2px_8px_rgba(0,0,0,0.25)]
              hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15 transition"          >
            <!-- Remove -->
            <button
              class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                     border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                     hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
              @click="removeProduct(item.id)"
              title="Remove product">
              <i class="bi bi-trash3"></i>
            </button>

            <h3 class="text-xl font-semibold text-neutral-100 pr-10">{{ item.name }}</h3>
            <p class="text-sm text-neutral-400 mt-1"><span class="text-neutral-300 font-medium">Code:</span> {{ item.code }}</p>
            <p class="text-sm text-neutral-400"><span class="text-neutral-300 font-medium">Category:</span> {{ item.productCategoryName }}</p>
            <p class="text-sm text-neutral-400"><span class="text-neutral-300 font-medium">Supplier:</span> {{ item.supplierName }}</p>


            <div class="mt-5 flex justify-end">
              <button
                @click="openProductDrawer(item)"
                class="inline-flex items-center gap-2 rounded-lg px-3.5 py-2 text-sm font-medium
                       border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                       shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_3px_10px_rgba(0,0,0,0.35)]
                       hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                       focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
                <i class="bi bi-eye"></i>
                View
              </button>
            </div>
          </div>
        </div>

        <!-- Empty state -->
        <div v-if="filteredProducts.length === 0" class="text-center text-neutral-400 mt-8">
          No products found.
        </div>
      </div>
    </section>

    <!-- 🟦 CENTERED MODAL (create/edit) -->
    <transition name="fade">
      <div v-if="showDrawer" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="showDrawer = false">
        <div
          class="w-full max-w-2xl rounded-2xl border border-neutral-700 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
                 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ drawerMode === 'edit' ? (activeEditProduct?.name || 'Edit Product') : 'Create New Product' }}
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                     border-1 border-neutral-700 bg-white/5 text-neutral-300
                     hover:bg-white/10 hover:text-white focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="showDrawer = false" title="Close">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <!-- Code -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Code</label>
              <input v-model="activeProduct!.code" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Name -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
              <input v-model="activeProduct!.name" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Price -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Price</label>
              <input v-model.number="activeProduct!.price" type="number"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Quantity -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Quantity</label>
              <input v-model.number="activeProduct!.quantity" type="number"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Unit -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Unit</label>
              <input v-model="activeProduct!.unit" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Volume -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Volume</label>
              <input v-model.number="activeProduct!.volume" type="number"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Volume unit -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Volume unit</label>
              <input v-model="activeProduct!.volumeUnit" type="text"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white
                            placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"/>
            </div>
            <!-- Category -->
            <div class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Category</label>
              <Multiselect
                v-model="selectedCategoryObj"
                :options="[{ id: 'All', name: 'All Categories' }, ...productCategories]"
                :custom-label="c => c.name"
                track-by="id"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="false"
                placeholder="All Categories"
                class="multiselect-dark w-[270px]"
              />
            </div>

            <!-- Supplier -->
            <div class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Supplier</label>
              <Multiselect
                v-model="selectedSupplierObj"
                :options="[{ id:'All', name:'All Suppliers' }, ...productSuppliers]"
                :custom-label="s => s.name"
                track-by="id"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="false"
                placeholder="All Suppliers"
                class="multiselect-dark w-[290px]"
              />
            </div>

            <!-- Is component -->
            <div class="sm:col-span-2">
              <label class="inline-flex items-center gap-3 text-medium text-neutral-300">
                <input type="checkbox" v-model="activeProduct!.isComponent"
                       class="h-4 w-4 rounded border-neutral-700 bg-neutral-900/70 text-cyan-400 focus:ring-cyan-400/30"/>
                Is Component (used in recipes)
              </label>
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
              @click="editProduct"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium
                     text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10">
              Update
            </button>
            <button
              v-else
              @click="createProduct"
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
  </main>
</template>

<style scoped>
/* pehme fade üleminek modaalile */
.fade-enter-active, .fade-leave-active { transition: opacity .18s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

/* --- vue-multiselect: tume, puhas --- */
:deep(.multiselect-dark) {
  @apply w-full rounded-xl border border-white/10 bg-neutral-900/70 text-white shadow-sm transition;
}

:deep(.multiselect-dark .multiselect__tags) {
  @apply flex items-center min-h-[44px] rounded-xl border-0 bg-transparent px-3 py-0;
}

:deep(.multiselect-dark .multiselect__placeholder),
:deep(.multiselect-dark .multiselect__single) {
  @apply block p-0 m-0 bg-transparent text-neutral-300 leading-[44px];
}

:deep(.multiselect-dark .multiselect__input) {
  @apply bg-transparent text-white placeholder-neutral-500 leading-[44px] p-0 m-0;
}

:deep(.multiselect-dark .multiselect__select),
:deep(.multiselect-dark .multiselect__clear) {
  @apply text-neutral-400 hover:text-white;
}

:deep(.multiselect-dark.multiselect--active .multiselect__tags) {
  @apply ring-2 ring-[#ffaa33]/35 outline-none border-[#ffaa33];
}

:deep(.multiselect-dark .multiselect__content-wrapper) {
  @apply mt-2 rounded-xl border border-white/10 bg-neutral-950/95 backdrop-blur supports-[backdrop-filter]:bg-neutral-950/80 shadow-2xl max-h-64;
}
:deep(.multiselect-dark .multiselect__content) { @apply py-1; }
:deep(.multiselect-dark .multiselect__option) { @apply px-4 py-2 text-neutral-200 cursor-pointer transition; }
:deep(.multiselect-dark .multiselect__option--highlight) { @apply bg-white/10; }
:deep(.multiselect-dark .multiselect__option--selected) { @apply bg-white/[0.06] text-[#ffaa33]; }
:deep(.multiselect-dark.multiselect--disabled) { @apply opacity-60 cursor-not-allowed; }

</style>
