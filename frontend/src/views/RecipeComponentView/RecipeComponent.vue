<script setup lang="ts">
import { onMounted, ref, computed, watch } from "vue";
import { X } from "lucide-vue-next";
import Multiselect from "vue-multiselect";
import "vue-multiselect/dist/vue-multiselect.css";

import { RecipeComponentService } from "@/services/mvcServices/RecipeComponentService.ts";
import { ProductService } from "@/services/mvcServices/ProductServices.ts";
import type { IRecipeComponent } from "@/domain/logic/IRecipeComponent.ts";
import type { IRecipeComponentEnriched } from "@/domain/logic/IRecipeComponentEnriched.ts";
import type { IProduct } from "@/domain/logic/IProduct.ts";

// Services
const recipeComponentService = new RecipeComponentService();
const productService = new ProductService();

// Data
const data = ref<IRecipeComponentEnriched[]>([]);
const allProducts = ref<IProduct[]>([]);
const loadingProducts = ref(false);

// Search
const searchName = ref("");

// Drawer state
const showDrawer = ref(false);
const drawerMode = ref<"edit" | "create">("edit");
const activeEditRecipeComponent = ref<IRecipeComponentEnriched | null>(null);
const activeCreateRecipeComponent = ref<IRecipeComponent | null>(null);

// Messages
const validationError = ref("");
const successMessage = ref("");

// Empty entity
const emptyRecipeComponent = ref<IRecipeComponent>({
  id: "",
  productRecipeId: "",
  componentProductId: "",
  amount: 0,
});

// Loaders
const loadProducts = async () => {
  loadingProducts.value = true;
  try {
    const result = await productService.getAllAsync();
    allProducts.value = result.data ?? [];
  } finally {
    loadingProducts.value = false;
  }
};

const fetchPageData = async () => {
  try {
    const result = await recipeComponentService.getEnrichedRecipeComponents();
    data.value = result.data || [];
  } catch (error) {
    console.error("Error fetching recipeComponents:", error);
  }
};

// initial load
onMounted(async () => {
  await Promise.all([loadProducts(), fetchPageData()]);
});

// Filtered list
const filteredRecipeComponent = computed(() => {
  const q = (searchName.value ?? "").trim().toLowerCase();
  if (!q) return data.value;

  return data.value.filter((rc) => {
    const fields = [
      rc.productRecipeName,
      rc.productRecipeCode,
      rc.componentProductName,
      rc.componentProductCode,
      rc.productRecipeId,
      rc.componentProductId,
    ];
    return fields.some((v) => (v ?? "").toString().toLowerCase().includes(q));
  });
});

// Group by recipe
type Grouped = {
  key: string;
  productRecipeName: string;
  items: IRecipeComponentEnriched[];
};

const groupedByRecipe = computed<Grouped[]>(() => {
  const map = new Map<string, Grouped>();
  for (const rc of filteredRecipeComponent.value) {
    const key = rc.productRecipeId;
    if (!map.has(key)) {
      map.set(key, {
        key,
        productRecipeName: rc.productRecipeName,
        items: [],
      });
    }
    map.get(key)!.items.push(rc);
  }
  return Array.from(map.values());
});

// -------- Multiselect state & syncing --------
type Opt = { id: string; label: string; raw: IProduct };
const toOpt = (p: IProduct): Opt => ({
  id: p.id,
  raw: p,
  label: `${p.name}${p.code ? ` (${p.code})` : ""}${p.unit ? ` · ${p.unit}` : ""}${p.volume ? ` · ${p.volume} ${p.volumeUnit ?? ""}` : ""}`,
});

const selectedRecipeProduct = ref<Opt | null>(null);
const selectedComponentProduct = ref<Opt | null>(null);

const recipeOptions = computed<Opt[]>(() =>
  allProducts.value.filter((p) => !p.isComponent).map(toOpt)
);

const componentOptions = computed<Opt[]>(() =>
  allProducts.value
    .filter((p) => p.isComponent)
    .filter((p) => !selectedRecipeProduct.value || p.id !== selectedRecipeProduct.value.id)
    .map(toOpt)
);

// selections -> model
watch(selectedRecipeProduct, (opt) => {
  if (!activeRecipeComponent.value) return;
  activeRecipeComponent.value.productRecipeId = opt?.id ?? "";
});

watch(selectedComponentProduct, (opt) => {
  if (!activeRecipeComponent.value) return;
  activeRecipeComponent.value.componentProductId = opt?.id ?? "";
});

// model -> selections (when opening drawer)
const hydrateSelectsFromModel = () => {
  const recId = activeRecipeComponent.value?.productRecipeId ?? "";
  const compId = activeRecipeComponent.value?.componentProductId ?? "";

  const rec = recId ? allProducts.value.find((p) => p.id === recId && !p.isComponent) : undefined;
  const comp = compId ? allProducts.value.find((p) => p.id === compId && p.isComponent) : undefined;

  selectedRecipeProduct.value = rec ? toOpt(rec) : null;
  selectedComponentProduct.value = comp ? toOpt(comp) : null;
};

// Active model computed
const activeRecipeComponent = computed({
  get() {
    return drawerMode.value === "edit"
      ? (activeEditRecipeComponent.value as unknown as IRecipeComponent | null)
      : activeCreateRecipeComponent.value;
  },
  set(value) {
    if (drawerMode.value === "edit")
      activeEditRecipeComponent.value = value as unknown as IRecipeComponentEnriched;
    else activeCreateRecipeComponent.value = value as IRecipeComponent;
  },
});

// Drawer actions
const openRecipeComponentEditDrawer = (recipeComponent: IRecipeComponentEnriched) => {
  activeEditRecipeComponent.value = { ...recipeComponent };
  drawerMode.value = "edit";
  showDrawer.value = true;
  hydrateSelectsFromModel();
};

const openRecipeComponentCreateDrawer = () => {
  activeCreateRecipeComponent.value = { ...emptyRecipeComponent.value };
  selectedRecipeProduct.value = null;
  selectedComponentProduct.value = null;
  drawerMode.value = "create";
  showDrawer.value = true;
};

// Save actions
const editRecipeComponent = async () => {
  if (!activeEditRecipeComponent.value) return;
  await recipeComponentService.updateAsync(activeEditRecipeComponent.value);
  showDrawer.value = false;
  await fetchPageData();
};

const createRecipeComponent = async () => {
  validationError.value = "";
  successMessage.value = "";

  try {
    if (!activeCreateRecipeComponent.value) return;
    const { id, ...rest } = activeCreateRecipeComponent.value;

    const cleaned = Object.fromEntries(
      Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
    ) as unknown as IRecipeComponent;

    const result = await recipeComponentService.addAsync(cleaned);
    if (result.errors?.length) {
      validationError.value = result.errors.join(", ");
    } else {
      successMessage.value = "✅ RecipeComponent has been successfully created!";
      showDrawer.value = false;
      await fetchPageData();
    }
  } catch (error) {
    console.error("Error creating recipeComponent:", error);
  }
};

// Remove
const removeRecipeComponent = async (id: string) => {
  if (!confirm("Are you sure you want to delete this recipeComponent?")) return;
  await recipeComponentService.removeAsync(id);
  await fetchPageData();
};
</script>

<template>
  <main class="product-wrapper">
    <section class="product-header">
      <div class="product-header-left">
        <h1 class="page-title">RecipeComponents</h1>
        <p class="subtitle">Browse and manage all RecipeComponents data</p>
      </div>
    </section>

    <div class="filter-bar">
      <div class="filter-controls">
        <input
          v-model="searchName"
          type="text"
          placeholder="Search by description"
          class="search-box"
        />
      </div>
      <button class="create-link" @click="openRecipeComponentCreateDrawer">+ Create New</button>
    </div>

    <div class="table-scroll-container">
      <div class="product-list">
        <div class="product-card" v-for="group in groupedByRecipe" :key="group.key">
          <div class="product-info">
            <h3 class="recipe-title">
              {{ group.productRecipeName }}
              <span class="component-count-badge">{{ group.items.length }}</span>
            </h3>

            <div class="components-wrap">
              <div
                v-for="item in group.items"
                :key="item.id"
                class="component-chip"
                :title="`${item.componentProductName} · ${item.amount} ${item.componentProductUnit || ''}`"
              >
                <button
                  class="chip-remove"
                  @click.stop="removeRecipeComponent(item.id)"
                  aria-label="Remove component"
                  title="Remove component"
                >
                  <X />
                </button>

                <span class="chip-name">{{ item.componentProductName }}</span>
                <span class="chip-dot">•</span>
                <span class="chip-amount">
                  {{ item.amount }}
                  <span v-if="item.componentProductUnit">{{ item.componentProductUnit }}</span>
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- DRAWER MODAL -->
    <transition name="fade">
      <div v-if="showDrawer" class="drawer-overlay" @click.self="showDrawer = false">
        <div class="drawer">
          <h2>{{ drawerMode === "edit" ? "Edit Recipe Component" : "Create New RecipeComponent" }}</h2>

          <label>Recipe Product</label>
          <Multiselect
            v-model="selectedRecipeProduct"
            :options="recipeOptions"
            track-by="id"
            label="label"
            :searchable="true"
            :loading="loadingProducts"
            :close-on-select="true"
            placeholder="Select recipe product (not a component)"
            class="ms"
          />

          <label>Component Product</label>
          <Multiselect
            v-model="selectedComponentProduct"
            :options="componentOptions"
            track-by="id"
            label="label"
            :searchable="true"
            :loading="loadingProducts"
            :close-on-select="true"
            placeholder="Select component product"
            class="ms"
          />

          <label>Amount</label>
          <input v-model.number="activeRecipeComponent!.amount" type="number" min="0" step="0.01" />

          <div class="drawer-buttons">
            <button v-if="drawerMode === 'edit'" @click="editRecipeComponent" class="update-btn">Update</button>
            <button v-else @click="createRecipeComponent" class="update-btn">Create</button>
            <button @click="showDrawer = false" class="cancel-btn">Cancel</button>
          </div>

          <p v-if="validationError" style="color:#ff7b7b; margin:0">{{ validationError }}</p>
          <p v-if="successMessage" style="color:#9effa5; margin:0">{{ successMessage }}</p>
        </div>
      </div>
    </transition>
  </main>
</template>

<style scoped>
html, body { height: 100%; margin: 0; overflow: hidden; }

/* Layout */
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
.product-header { display: flex; justify-content: space-between; align-items: flex-end; flex-wrap: wrap; }
.product-header-left { display: flex; flex-direction: column; gap: 0.3rem; }
.page-title { font-size: 2.6rem; font-weight: 800; color: #ffaa33; text-shadow: 0 0 10px rgba(255, 170, 51, 0.25); margin: 0; }
.subtitle { font-size: 1rem; color: #bbb; margin: 0; opacity: 0.85; }

/* Filter Bar */
.filter-bar {
  display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;
  background: rgba(30, 30, 30, 0.6); backdrop-filter: blur(8px); padding: 1rem 1.5rem; border-radius: 16px;
  box-shadow: 0 0 12px rgba(255, 170, 51, 0.05);
}
.filter-controls { display: flex; gap: 1rem; flex-wrap: wrap; }
.filter-bar select, .search-box {
  padding: 0.6rem 1rem; font-size: 1rem; border-radius: 12px; border: 1px solid #ffaa33;
  background-color: rgba(43, 43, 43, 0.5); color: white; min-width: 220px; transition: all 0.2s ease;
}
.filter-bar select:focus { outline: none; border-color: #ffc266; background-color: rgb(43, 43, 43); }
.filter-bar input::placeholder { color: #ccc; }

/* Create Button */
.create-link {
  background: linear-gradient(to right, #ffaa33, #ff8c00); color: black; padding: 0.6rem 1.4rem;
  border-radius: 12px; font-weight: 700; text-decoration: none; box-shadow: 0 2px 6px rgba(255, 165, 0, 0.2);
  transition: all 0.2s ease;
}
.create-link:hover { background: linear-gradient(to right, #ffc56e, #ffa726); box-shadow: 0 3px 10px rgba(255, 165, 0, 0.3); }

/* Cards */
.table-scroll-container {
  flex-grow: 1; overflow-y: auto; padding: 1.2rem; margin-top: 1rem; border-radius: 16px;
  background: rgba(20, 20, 20, 0.5); backdrop-filter: blur(6px); box-shadow: inset 0 0 20px rgba(255, 165, 0, 0.05);
}
.product-list { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1.5rem; }
.product-card {
  position: relative; background: linear-gradient(180deg, rgba(45,45,45,.72), rgba(30,30,30,.62));
  border-radius: 14px; padding: 1rem; border: 1px solid rgba(255,255,255,.06);
  transition: all 0.3s ease; display: flex; flex-direction: column; justify-content: space-between;
}
.product-card:hover { box-shadow: 0 6px 16px rgba(255, 170, 51, 0.1); transform: translateY(-3px); border-color: rgba(255,170,51,.55); }
.product-info h3 { color: #ffaa33; font-size: 1.1rem; margin-bottom: 0.4rem; }

/* Chips */
.recipe-title { display: flex; align-items: center; gap: .6rem; color: #ffaa33; font-size: 1.15rem; margin: 0 0 .6rem 0; }
.component-count-badge {
  display: inline-flex; align-items: center; justify-content: center; min-width: 24px; height: 22px; padding: 0 .4rem;
  border-radius: 999px; font-size: .8rem; font-weight: 700; color: black;
  background: linear-gradient(to right, #ffaa33, #ff8c00); box-shadow: 0 2px 6px rgba(255,165,0,.25);
}
.components-wrap { display: flex; flex-wrap: wrap; gap: .5rem .6rem; margin-top: .2rem; }
.component-chip {
  position: relative; display: inline-flex; align-items: center; gap: .45rem; padding: .45rem .65rem .45rem .45rem;
  border-radius: 999px; backdrop-filter: blur(6px);
  background: radial-gradient(120% 140% at 10% 10%, rgba(255,170,51,.20) 0%, rgba(255,170,51,.06) 45%, rgba(255,255,255,.03) 100%);
  border: 1px solid rgba(255,170,51,.25); transition: transform .12s ease, box-shadow .12s ease, border-color .12s ease;
}
.component-chip:hover { transform: translateY(-1px); border-color: rgba(255,170,51,.6); box-shadow: 0 6px 14px rgba(255,170,51,.12); }
.chip-name { font-weight: 600; letter-spacing: .1px; color: #f4f4f4; }
.chip-dot { opacity: .5; }
.chip-amount { font-weight: 700; color: #ffc266; }
.chip-remove {
  display: inline-flex; align-items: center; justify-content: center; width: 22px; height: 22px; border-radius: 999px;
  border: 1px solid rgba(255, 80, 80, .55); background: rgba(255, 80, 80, .12); color: #ff6b6b; margin-right: .25rem;
  transition: background .12s ease, transform .12s ease, border-color .12s ease;
}
.chip-remove:hover { background: rgba(255, 80, 80, .22); border-color: rgba(255, 80, 80, .9); color: white; transform: scale(1.03); }

/* Drawer */
.drawer { width: 420px; background: linear-gradient(to bottom, #1e1e1e, #121212); color: white; padding: 2rem; overflow-y: auto; height: 100%;
  box-shadow: -8px 0 20px rgba(0, 0, 0, 0.5); display: flex; flex-direction: column; gap: 1.2rem; border-left: 1px solid rgba(255, 255, 255, 0.05); }
.drawer input {
  width: 100%; padding: 0.6rem 1rem; border-radius: 10px; border: none; background: rgba(60, 60, 60, 0.7);
  color: white; font-size: 1rem; transition: all 0.2s;
}
.drawer input:focus { outline: none; background: rgba(80, 80, 80, 0.85); border: 1px solid #ffaa33; }
.drawer-buttons { display: flex; justify-content: flex-end; gap: 1rem; margin-top: 1.5rem; }
.update-btn { background: linear-gradient(to right, #ffaa33, #ff8c00); color: black; font-size: 0.95rem; padding: 0.6rem 1.4rem; border-radius: 10px; font-weight: 700; border: none; cursor: pointer; box-shadow: 0 3px 8px rgba(0, 0, 0, 0.2); }
.cancel-btn { background: #333; color: white; font-size: 0.95rem; padding: 0.6rem 1.4rem; border-radius: 10px; border: 1px solid rgba(255,255,255,0.1); cursor: pointer; transition: all 0.2s ease; }
.cancel-btn:hover { background: #444; }
.drawer-overlay { position: fixed; inset: 0; background: rgba(0, 0, 0, 0.6); display: flex; justify-content: flex-end; z-index: 999; }

/* vue-multiselect theming */
.ms .multiselect { background: rgba(60,60,60,0.7); border: 1px solid rgba(255,255,255,0.08); border-radius: 10px; color: #fff; }
.ms .multiselect__tags { background: transparent; border: none; min-height: 44px; padding: .45rem .7rem; }
.ms .multiselect__input, .ms .multiselect__single { color: #fff; }
.ms .multiselect__content-wrapper { background: #1e1e1e; border: 1px solid rgba(255,255,255,0.08); }
.ms .multiselect__option--highlight { background: linear-gradient(to right, #ffaa33, #ff8c00); color: #000; }
.ms .multiselect__option--selected { background: rgba(255,170,51,.18); color: #ffd9a1; }

/* Responsive */
@media (max-width: 768px) {
  .product-header { flex-direction: column; align-items: flex-start; gap: 1rem; }
  .filter-bar { flex-direction: column; }
  .product-list { grid-template-columns: 1fr; }
}
</style>
