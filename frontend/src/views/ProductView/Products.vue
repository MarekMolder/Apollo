<script setup lang="ts">
import { onMounted, ref, computed, watch } from "vue";
import type { IProductEnriched } from "@/domain/logic/IProductEnriched";
import { ProductService } from "@/services/mvcServices/ProductServices";
import type { IProduct } from "@/domain/logic/IProduct.ts";
import { ProductCategoryService } from "@/services/mvcServices/ProductCategoryService.ts";
import type { IProductCategory } from "@/domain/logic/IProductCategory.ts";
import products from "@/views/ProductView/Products.vue";
import Multiselect from "vue-multiselect";
import "vue-multiselect/dist/vue-multiselect.min.css";
import type { ISupplier } from "@/domain/logic/ISupplier.ts";
import { SupplierService } from "@/services/mvcServices/SupplierService.ts";
import { useSidebarStore } from "@/stores/sidebarStore";
const sidebarStore = useSidebarStore();
const showHelp = ref(false);

// Services
const service = new ProductService();
const productCategoryService = new ProductCategoryService();
const supplierService = new SupplierService();

// Entity's
const data = ref<IProductEnriched[]>([]);
const productCategories = ref<IProductCategory[]>([]);
const productSuppliers = ref<ISupplier[]>([]);

// ---------------- Filters (ERALDI state) ----------------
const filterCategoryId = ref<"All" | string>("All");
const filterSupplierId = ref<"All" | string>("All");
const searchCode = ref("");
const searchName = ref("");
const categories = ref<string[]>([]);
const suppliers = ref<string[]>([]);

// ---------------- Drawer / Form state ----------------
const showDrawer = ref(false);
const drawerMode = ref<"edit" | "create">("edit");
const activeEditProduct = ref<IProductEnriched | null>(null);
const activeCreateProduct = ref<IProduct | null>(null);

// Messages
const validationError = ref("");
const successMessage = ref("");

// Empty Product entity
const emptyProduct = ref<IProduct>({
  id: "",
  unit: "",
  volume: 0,
  volumeUnit: "",
  code: "",
  name: "",
  price: 0,
  quantity: 1,
  productCategoryId: "",
  isComponent: false,
  supplierId: "",
});

// Init
onMounted(async () => {
  products.value = (await service.getEnrichedProducts()).data || [];
  productCategories.value = (await productCategoryService.getAllAsync()).data || [];
  productSuppliers.value = (await supplierService.getAllAsync()).data || [];
});

const fetchPageData = async () => {
  try {
    const result = await service.getEnrichedProducts();
    data.value = result.data || [];

    categories.value = ["All", ...new Set(data.value.map((item) => item.productCategoryName))];
    suppliers.value = ["All", ...new Set(data.value.map((item) => item.supplierName))];
  } catch (error) {
    console.error("Error fetching products:", error);
  }
};
onMounted(fetchPageData);

// Filtered list
const filteredProducts = computed(() =>
  data.value.filter((product) => {
    const matchCategory =
      filterCategoryId.value === "All" ||
      String(product.productCategoryId) === String(filterCategoryId.value);

    const matchSupplier =
      filterSupplierId.value === "All" ||
      String(product.supplierId) === String(filterSupplierId.value);

    const matchSearchCode = product.code.toLowerCase().includes(searchCode.value.toLowerCase());
    const matchSearchName = product.name.toLowerCase().includes(searchName.value.toLowerCase());

    return matchCategory && matchSupplier && matchSearchCode && matchSearchName;
  })
);

// ---------------- Drawer actions ----------------
const openProductDrawer = (product: IProductEnriched) => {
  activeEditProduct.value = { ...product };
  // ÄRA muuda filtreid siin
  drawerMode.value = "edit";
  showDrawer.value = true;
};

const openCreateDrawer = () => {
  activeCreateProduct.value = JSON.parse(JSON.stringify(emptyProduct.value));
  // ÄRA muuda filtreid siin
  drawerMode.value = "create";
  showDrawer.value = true;
};

const activeProduct = computed({
  get() {
    return drawerMode.value === "edit" ? activeEditProduct.value : activeCreateProduct.value;
  },
  set(value) {
    if (drawerMode.value === "edit") activeEditProduct.value = value as IProductEnriched;
    else activeCreateProduct.value = value as IProduct;
  },
});

// ---------------- Create / Edit ----------------

const createProduct = async () => {
  validationError.value = "";
  successMessage.value = "";

  try {
    if (!activeCreateProduct.value) return;

    const { id, ...rest } = activeCreateProduct.value;

    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
    ) as unknown as IProduct;

    const result = await service.addAsync(cleaned);

    if (result.errors?.length) {
      // short message in UI
      validationError.value = "❌ Failed to create product. Please check the fields.";
      // full error only in console
      console.error("Product creation validation errors:", result.errors);
    } else {
      successMessage.value = "✅ Product has been successfully created!";
      showDrawer.value = false;
      await fetchPageData();
    }
  } catch (error) {
    validationError.value = "❌ An unexpected error occurred while creating the product.";
    console.error("Error creating product:", error);
  }
};

const editProduct = async () => {
  if (!activeEditProduct.value) return;
  try {
    await service.updateAsync(activeEditProduct.value);
    showDrawer.value = false;
    await fetchPageData();
  } catch (err) {
    validationError.value = "❌ Failed to update product.";
    console.error("Failed to update product:", err);
  }
};

// Remove
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

// ---------------- FORM Multiselect computed (EI MÕJUTA filtreid) ----------------
const formSupplierObj = computed<ISupplier | null>({
  get() {
    const sid =
      drawerMode.value === "edit"
        ? activeEditProduct.value?.supplierId
        : activeCreateProduct.value?.supplierId;

    if (!sid) return null;
    return productSuppliers.value.find((s) => s.id === sid) ?? null;
  },
  set(val) {
    const id = val?.id ?? "";
    if (drawerMode.value === "edit" && activeEditProduct.value) {
      activeEditProduct.value.supplierId = id;
    }
    if (drawerMode.value === "create" && activeCreateProduct.value) {
      activeCreateProduct.value.supplierId = id;
    }
  },
});

const formCategoryObj = computed<IProductCategory | null>({
  get() {
    const pid =
      drawerMode.value === "edit"
        ? activeEditProduct.value?.productCategoryId
        : activeCreateProduct.value?.productCategoryId;

    if (!pid) return null;
    return productCategories.value.find((c) => c.id === pid) ?? null;
  },
  set(val) {
    const id = val?.id ?? "";
    if (drawerMode.value === "edit" && activeEditProduct.value) {
      activeEditProduct.value.productCategoryId = id;
    }
    if (drawerMode.value === "create" && activeCreateProduct.value) {
      activeCreateProduct.value.productCategoryId = id;
    }
  },
});

// ---------------- Unit / Volume loogika (create) ----------------
const availableUnits = ["g", "kg", "mg", "ml", "l", "cl", "tk"];
const isCreateMode = computed(() => drawerMode.value === "create");
const cp = computed(() => activeCreateProduct.value);

// kui CREATE + unit != 'tk' -> volumeUnit = unit; volume = quantity
watch(
  () => [isCreateMode.value, cp.value?.unit, cp.value?.quantity],
  () => {
    if (!isCreateMode.value || !cp.value) return;
    const u = cp.value.unit?.trim();
    if (u && u !== "tk") {
      cp.value.volumeUnit = u;
      cp.value.volume = Number(cp.value.quantity) || 0;
    }
  },
  { immediate: true }
);

// kui CREATE + unit === 'tk' -> luba sisestada volume ja volumeUnit (anna mõistlikud defaultid)
watch(
  () => cp.value?.unit,
  (u) => {
    if (!isCreateMode.value || !cp.value) return;
    if (u === "tk") {
      cp.value.volume ??= 0;
      cp.value.volumeUnit ||= "ml";
    }
  }
);
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
               relative inline-block"
      >
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
        <div class="mb-6 flex flex-wrap gap-3 items-center justify-between overflow-x-auto px-2">
          <!-- vasak blokk: filtrid -->
          <div class="flex flex-wrap gap-3">
            <!-- Category filter -->
            <div class="relative w-48">
              <label class="sr-only">Category</label>
              <select
                v-model="filterCategoryId"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30 focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9"
              >
                <option value="All">All Categories</option>
                <option v-for="c in productCategories" :key="c.id" :value="c.id">{{ c.name }}</option>
              </select>
              <i class="bi bi-list-ul absolute right-8 top-1/2 -translate-y-1/2 text-neutral-400"></i>
              <i class="bi bi-chevron-down pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>

            <!-- Supplier filter -->
            <div class="relative w-56">
              <label class="sr-only">Supplier</label>
              <select
                v-model="filterSupplierId"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white px-3 h-11 text-medium focus:outline-none focus:ring-2 focus:ring-cyan-400/30 focus:border-neutral-500 transition shadow-inner shadow-black/30 pr-9"
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
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white h-11 text-medium pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 focus:border-neutral-500 transition shadow-inner shadow-black/30"
              />
              <i class="bi bi-upc-scan pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>

            <!-- Search by name -->
            <div class="relative w-48">
              <input
                v-model="searchName"
                type="text"
                placeholder="Search by name"
                class="w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white h-11 text-medium pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 focus:border-neutral-500 transition shadow-inner shadow-black/30"
              />
              <i class="bi bi-search pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
            </div>
          </div>

          <!-- parem blokk: nupp -->
          <div class="flex-shrink-0">
            <button
              @click="openCreateDrawer"
              class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200 shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)] hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition"
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
            class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700 shadow-[0_2px_8px_rgba(0,0,0,0.25)] hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/15 transition"
          >
            <!-- Remove -->
            <button
              class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8 border-1 border-rose-400/50 bg-rose-500/10 text-rose-300 hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
              @click="removeProduct(item.id)"
              title="Remove product"
            >
              <i class="bi bi-trash3"></i>
            </button>

            <h3 class="text-xl font-semibold text-neutral-100 pr-10">{{ item.name }}</h3>
            <p class="text-sm text-neutral-400 mt-1">
              <span class="text-neutral-300 font-medium">Code:</span> {{ item.code }}
            </p>
            <p class="text-sm text-neutral-400">
              <span class="text-neutral-300 font-medium">Category:</span> {{ item.productCategoryName }}
            </p>
            <p class="text-sm text-neutral-400">
              <span class="text-neutral-300 font-medium">Supplier:</span> {{ item.supplierName }}
            </p>

            <div class="mt-5 flex justify-end">
              <button
                @click="openProductDrawer(item)"
                class="inline-flex items-center gap-2 rounded-lg px-3.5 py-2 text-sm font-medium border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200 shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_3px_10px_rgba(0,0,0,0.35)] hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition"
              >
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
      <div
        v-if="showDrawer"
        class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4"
        @click.self="showDrawer = false"
      >
        <div
          class="w-full max-w-2xl rounded-2xl border border-neutral-700 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]"
        >
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ drawerMode === "edit" ? (activeEditProduct?.name || "Edit Product") : "Create New Product" }}
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-neutral-700 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white focus:outline-none focus:ring-2 focus:ring-white/15"
              @click="showDrawer = false"
              title="Close"
            >
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <!-- Code -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Code</label>
              <input
                v-model="activeProduct!.code"
                type="text"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              />
            </div>
            <!-- Name -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
              <input
                v-model="activeProduct!.name"
                type="text"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              />
            </div>
            <!-- Price -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Price</label>
              <input
                v-model.number="activeProduct!.price"
                type="number"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              />
            </div>
            <!-- Quantity -->
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Quantity</label>
              <input
                v-model.number="activeProduct!.quantity"
                type="number"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              />
            </div>

            <!-- Unit (SELECT) -->
            <div class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Unit</label>
              <Multiselect
                v-model="(activeProduct as any).unit"
                :options="availableUnits"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="false"
                placeholder="Select unit"
                class="multiselect-dark w-[270px]"
              />
              <p v-if="isCreateMode && activeCreateProduct?.unit !== 'tk'" class="mt-1 text-xs text-neutral-400">
                In create mode: volume = quantity, volume unit = unit.
              </p>
            </div>

            <!-- Volume (näita ainult siis, kui CREATE ja unit === 'tk'; muidu täidetakse automaatselt) -->
            <div v-if="!(isCreateMode && activeCreateProduct?.unit !== 'tk')">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Volume</label>
              <input
                v-model.number="activeProduct!.volume"
                type="number"
                min="0"
                step="0.001"
                class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-medium text-white placeholder-neutral-500 outline-none transition focus:border-cyan-400/40 focus:ring-2 focus:ring-cyan-400/20"
              />
            </div>

            <!-- Volume unit (FORM, multiselect) -->
            <div v-if="!(isCreateMode && activeCreateProduct?.unit !== 'tk')" class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Volume unit</label>
              <Multiselect
                v-model="(activeProduct as any).volumeUnit"
                :options="availableUnits.filter(u => u !== 'tk')"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="true"
                placeholder="Select volume unit"
                class="multiselect-dark w-[270px]"
              />
            </div>

            <!-- Category (FORM) -->
            <div class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Category</label>
              <Multiselect
                v-model="formCategoryObj"
                :options="productCategories"
                :custom-label="c => c.name"
                track-by="id"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="true"
                placeholder="Select category"
                class="multiselect-dark w-[270px]"
              />
            </div>

            <!-- Supplier (FORM) -->
            <div class="relative shrink-0">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Supplier</label>
              <Multiselect
                v-model="formSupplierObj"
                :options="productSuppliers"
                :custom-label="s => s.name"
                track-by="id"
                :searchable="true"
                :close-on-select="true"
                :allow-empty="true"
                placeholder="Select supplier"
                class="multiselect-dark w-[290px]"
              />
            </div>

            <!-- Is component -->
            <div class="sm:col-span-2">
              <label class="inline-flex items-center gap-3 text-medium text-neutral-300">
                <input
                  type="checkbox"
                  v-model="activeProduct!.isComponent"
                  class="h-4 w-4 rounded border-neutral-700 bg-neutral-900/70 text-cyan-400 focus:ring-cyan-400/30"
                />
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
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              Update
            </button>
            <button
              v-else
              @click="createProduct"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              Create
            </button>
            <button
              @click="showDrawer = false"
              class="inline-flex items-center justify-center rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              Cancel
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- 🟣 FLOATING HELP BUTTON -->
    <button
      @click="showHelp = true"
      class="fixed z-[1100] bottom-6 right-6 w-12 h-12 rounded-full
         bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent
         border-1 border-neutral-700 text-neutral-100
         shadow-[0_6px_24px_rgba(0,0,0,0.45)]
         hover:from-cyan-500/30 hover:via-cyan-400/20
         backdrop-blur-sm transition focus:outline-none
         focus:ring-2 focus:ring-cyan-400/40"
      aria-label="Help & guide"
      title="Help"
    >
      <i class="bi bi-question-lg text-xl"></i>
    </button>

    <!-- 🟣 HELP MODAL -->
    <transition name="fade">
      <div
        v-if="showHelp"
        class="fixed inset-0 z-[1200] flex items-center justify-center bg-black/60 p-4"
        @click.self="showHelp = false"
      >
        <div
          class="w-full max-w-3xl rounded-2xl border border-white/10
             bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8
             shadow-[0_20px_60px_rgba(0,0,0,0.6)]"
          role="dialog"
          aria-modal="true"
          aria-labelledby="help-title"
        >
          <!-- Header -->
          <div class="flex items-start justify-between gap-4">
            <h2 id="help-title" class="text-2xl font-bold tracking-tight text-neutral-100">
              Kuidas seda lehte kasutada?
            </h2>
            <button
              class="inline-flex items-center justify-center w-9 h-9 rounded-xl
                 border border-white/10 bg-white/5 text-neutral-300
                 hover:bg-white/10 hover:text-white focus:outline-none
                 focus:ring-2 focus:ring-white/15"
              @click="showHelp = false"
              title="Sulge"
              aria-label="Close help"
            >
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <!-- Body -->
          <div class="mt-5 space-y-4 text-neutral-200 leading-relaxed">
            <p>
              See leht võimaldab <strong>otsida</strong>, <strong>filtreerida</strong>, <strong>lisada</strong>, <strong>muuta</strong> ja
              <strong>kustutada</strong> tooteid. Ülariba filtritega saad kiiresti leida vajaliku.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Filtrid:</strong> vali <em>Category</em> ja/või <em>Supplier</em> või kasuta välju
                <em>Search by code</em> ja <em>Search by name</em>. Valik <em>All</em> näitab kõiki tulemusi.
              </li>
              <li>
                <strong>Uus toode:</strong> klõpsa <em>New Product</em>, täida vorm (Code, Name, Price, Quantity, Unit, Category, Supplier) ja salvesta.
              </li>
              <li>
                <strong>Vaatamine/muutmine:</strong> kaardil nupp <em>View</em> avab vormi, kus saad toote andmeid muuta.
              </li>
              <li>
                <strong>Kustutamine:</strong> prügikasti ikoon eemaldab toote pärast kinnitust. Seda toimingut ei saa tagasi võtta.
              </li>
              <li>
                <strong>Unit &amp; Volume loogika (create):</strong>
                kui <em>Unit ≠ tk</em>, siis määratakse <em>volume = quantity</em> ja <em>volumeUnit = unit</em> automaatselt.
                Kui <em>Unit = tk</em>, saad <em>Volume</em> ja <em>Volume unit</em> käsitsi valida (nt ml, l, g).
              </li>
              <li>
                <strong>Is Component:</strong> märgi toode komponendiks, kui seda kasutatakse retseptides (Recipe Components).
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaali saab sulgeda taustale klõpsates või ülanurga <em>×</em> nupust. Filtrid ei muutu, kui avad/lood toote —
              nii on mugav jätkata samas vaates.
            </p>
          </div>

          <!-- Footer -->
          <div class="mt-6 flex justify-end">
            <button
              @click="showHelp = false"
              class="inline-flex items-center justify-center rounded-xl border border-neutral-700
                 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200
                 hover:bg-white/10 focus:outline-none focus:ring-4 focus:ring-white/10"
            >
              Sain aru
            </button>
          </div>
        </div>
      </div>
    </transition>

  </main>
</template>

<style scoped>
/* pehme fade üleminek modaalile */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.18s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

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
:deep(.multiselect-dark .multiselect__content) {
  @apply py-1;
}
:deep(.multiselect-dark .multiselect__option) {
  @apply px-4 py-2 text-neutral-200 cursor-pointer transition;
}
:deep(.multiselect-dark .multiselect__option--highlight) {
  @apply bg-white/10;
}
:deep(.multiselect-dark .multiselect__option--selected) {
  @apply bg-white/[0.06] text-[#ffaa33];
}
:deep(.multiselect-dark.multiselect--disabled) {
  @apply opacity-60 cursor-not-allowed;
}
</style>
