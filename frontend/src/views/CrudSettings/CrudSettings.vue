<script setup lang="ts">
import { ref, computed, onMounted, watch } from "vue";
import { X } from "lucide-vue-next";
import Multiselect from "vue-multiselect";
import "vue-multiselect/dist/vue-multiselect.min.css";
import type { Ref } from "vue";
import { useSidebarStore } from '@/stores/sidebarStore';
const sidebarStore = useSidebarStore();
const showHelp = ref(false);

// --- Services & Types ---
import { ActionTypeService } from "@/services/mvcServices/ActionTypeService";
import type { IActionType } from "@/domain/logic/IActionType";
import { ReasonService } from "@/services/mvcServices/ReasonService";
import type { IReason } from "@/domain/logic/IReason";
import { ProductCategoryService } from "@/services/mvcServices/ProductCategoryService";
import type { IProductCategory } from "@/domain/logic/IProductCategory";
import { AddressService } from "@/services/mvcServices/AddressService";
import type { IAddress } from "@/domain/logic/IAddress";
import type { IAddressEnriched } from "@/domain/logic/IAddressEnriched";
import { RecipeComponentService } from "@/services/mvcServices/RecipeComponentService";
import type { IRecipeComponent } from "@/domain/logic/IRecipeComponent";
import type { IRecipeComponentEnriched } from "@/domain/logic/IRecipeComponentEnriched";
import { ProductService } from "@/services/mvcServices/ProductServices";
import type { IProduct } from "@/domain/logic/IProduct";

// ---------------- Tabs ----------------
type TabKey = "actionTypes" | "reasons" | "productCategories" | "addresses" | "recipeComponents";
const tabs: { key: TabKey; label: string }[] = [
  { key: "actionTypes", label: "Action Types" },
  { key: "reasons", label: "Reasons" },
  { key: "productCategories", label: "Product Categories" },
  { key: "addresses", label: "Addresses" },
  { key: "recipeComponents", label: "Recipe Components" },
];
const activeTab = ref<TabKey>("actionTypes");

// ---------------------------------------------------
// ACTION TYPES
// ---------------------------------------------------
const atService = new ActionTypeService();
const at_data = ref<IActionType[]>([]);
const at_search = ref("");
const at_show = ref(false);
const at_mode = ref<"edit" | "create">("edit");
const at_edit = ref<IActionType | null>(null);
const at_create = ref<IActionType | null>(null);
const at_empty: IActionType = { id: "", name: "", code: 0 };
const at_validation = ref("");
const at_success = ref("");

const at_filtered = computed(() =>
  at_data.value.filter((x) =>
    x.name.toLowerCase().includes(at_search.value.toLowerCase())
  )
);

const at_fetch = async () => {
  const res = await atService.getAllAsync();
  at_data.value = res.data ?? [];
};
const at_openEdit = (row: IActionType) => {
  at_edit.value = { ...row, code: 2 };
  at_mode.value = "edit";
  at_show.value = true;
};
const at_openCreate = () => {
  at_create.value = { ...at_empty, code: 2 };
  at_mode.value = "create";
  at_show.value = true;
};
const at_active = computed<IActionType | null>({
  get() {
    return at_mode.value === "edit" ? at_edit.value : at_create.value;
  },
  set(v) {
    if (at_mode.value === "edit") at_edit.value = v;
    else at_create.value = v;
  },
});
const at_update = async () => {
  if (!at_edit.value) return;
  await atService.updateAsync(at_edit.value);
  at_show.value = false;
  await at_fetch();
};
const at_createFn = async () => {
  at_validation.value = "";
  at_success.value = "";
  if (!at_create.value) return;
  const { id, ...rest } = at_create.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
  ) as unknown as IActionType;
  const result = await atService.addAsync(cleaned);
  if (result.errors?.length) at_validation.value = result.errors.join(", ");
  else {
    at_success.value = "✅ Action Type created!";
    at_show.value = false;
    await at_fetch();
  }
};
const at_remove = async (id: string) => {
  if (!confirm("Delete this Action Type?")) return;
  await atService.removeAsync(id);
  await at_fetch();
};

// ---------------------------------------------------
// REASONS
// ---------------------------------------------------
const rsService = new ReasonService();
const rs_data = ref<IReason[]>([]);
const rs_search = ref("");
const rs_show = ref(false);
const rs_mode = ref<"edit" | "create">("edit");
const rs_edit = ref<IReason | null>(null);
const rs_create = ref<IReason | null>(null);
const rs_empty: IReason = { id: "", description: "" };

const rs_filtered = computed(() =>
  rs_data.value.filter((x) =>
    x.description.toLowerCase().includes(rs_search.value.toLowerCase())
  )
);
const rs_fetch = async () => {
  const res = await rsService.getAllAsync();
  rs_data.value = res.data ?? [];
};
const rs_openEdit = (row: IReason) => {
  rs_edit.value = { ...row };
  rs_mode.value = "edit";
  rs_show.value = true;
};
const rs_openCreate = () => {
  rs_create.value = { ...rs_empty };
  rs_mode.value = "create";
  rs_show.value = true;
};
const rs_active = computed<IReason | null>({
  get() {
    return rs_mode.value === "edit" ? rs_edit.value : rs_create.value;
  },
  set(v) {
    if (rs_mode.value === "edit") rs_edit.value = v;
    else rs_create.value = v;
  },
});
const rs_update = async () => {
  if (!rs_edit.value) return;
  await rsService.updateAsync(rs_edit.value);
  rs_show.value = false;
  await rs_fetch();
};
const rs_createFn = async () => {
  if (!rs_create.value) return;
  const { id, ...rest } = rs_create.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
  ) as unknown as IReason;
  const res = await rsService.addAsync(cleaned);
  if (res.errors?.length) alert(res.errors.join(", "));
  else {
    rs_show.value = false;
    await rs_fetch();
  }
};
const rs_remove = async (id: string) => {
  if (!confirm("Delete this Reason?")) return;
  await rsService.removeAsync(id);
  await rs_fetch();
};

// ---------------------------------------------------
// PRODUCT CATEGORIES
// ---------------------------------------------------
const pcService = new ProductCategoryService();
const pc_data = ref<IProductCategory[]>([]);
const pc_search = ref("");
const pc_show = ref(false);
const pc_mode = ref<"edit" | "create">("edit");
const pc_edit = ref<IProductCategory | null>(null);
const pc_create = ref<IProductCategory | null>(null);
const pc_empty: IProductCategory = { id: "", name: "" };

const pc_filtered = computed(() =>
  pc_data.value.filter((x) =>
    x.name.toLowerCase().includes(pc_search.value.toLowerCase())
  )
);
const pc_fetch = async () => {
  const res = await pcService.getAllAsync();
  pc_data.value = res.data ?? [];
};
const pc_openEdit = (row: IProductCategory) => {
  pc_edit.value = { ...row };
  pc_mode.value = "edit";
  pc_show.value = true;
};
const pc_openCreate = () => {
  pc_create.value = { ...pc_empty };
  pc_mode.value = "create";
  pc_show.value = true;
};
const pc_active = computed<IProductCategory | null>({
  get() {
    return pc_mode.value === "edit" ? pc_edit.value : pc_create.value;
  },
  set(v) {
    if (pc_mode.value === "edit") pc_edit.value = v;
    else pc_create.value = v;
  },
});
const pc_update = async () => {
  if (!pc_edit.value) return;
  await pcService.updateAsync(pc_edit.value);
  pc_show.value = false;
  await pc_fetch();
};
const pc_createFn = async () => {
  if (!pc_create.value) return;
  const { id, ...rest } = pc_create.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
  ) as unknown as IProductCategory;
  const res = await pcService.addAsync(cleaned);
  if (res.errors?.length) alert(res.errors.join(", "));
  else {
    pc_show.value = false;
    await pc_fetch();
  }
};
const pc_remove = async (id: string) => {
  if (!confirm("Delete this Product Category?")) return;
  await pcService.removeAsync(id);
  await pc_fetch();
};

// ---------------------------------------------------
// ADDRESSES
// ---------------------------------------------------
const addrService = new AddressService();
const addr_data = ref<IAddressEnriched[]>([]);
const addr_search = ref("");
const addr_show = ref(false);
const addr_mode = ref<"edit" | "create">("edit");
const addr_edit = ref<IAddress | null>(null);
const addr_create = ref<IAddress | null>(null);
const addr_empty: IAddress = {
  id: "",
  name: "",
  streetName: "",
  buildingNr: 0,
  unitNr: null,
  postalCode: "",
  city: "",
  province: "",
  country: "",
};

const addr_filtered = computed(() =>
  addr_data.value.filter((x) =>
    (x.name ?? "").toLowerCase().includes(addr_search.value.toLowerCase())
  )
);
const addr_fetch = async () => {
  const res = await addrService.getEnrichedAddresses();
  addr_data.value = res.data ?? [];
};
const addr_openEdit = (row: IAddressEnriched) => {
  const { id, name, streetName, buildingNr, unitNr, postalCode, city, province, country } = row;
  addr_edit.value = { id, name, streetName, buildingNr, unitNr, postalCode, city, province, country };
  addr_mode.value = "edit";
  addr_show.value = true;
};
const addr_openCreate = () => {
  addr_create.value = { ...addr_empty };
  addr_mode.value = "create";
  addr_show.value = true;
};
const addr_active = computed<IAddress | null>({
  get() {
    return addr_mode.value === "edit" ? addr_edit.value : addr_create.value;
  },
  set(v) {
    if (addr_mode.value === "edit") addr_edit.value = v;
    else addr_create.value = v;
  },
});
const addr_update = async () => {
  if (!addr_edit.value) return;
  await addrService.updateAsync(addr_edit.value);
  addr_show.value = false;
  await addr_fetch();
};
const addr_createFn = async () => {
  if (!addr_create.value) return;
  const { id, ...rest } = addr_create.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
  ) as unknown as IAddress;
  const res = await addrService.addAsync(cleaned);
  if (res.errors?.length) alert(res.errors.join(", "));
  else {
    addr_show.value = false;
    await addr_fetch();
  }
};
const addr_remove = async (id: string) => {
  if (!confirm("Delete this Address?")) return;
  await addrService.removeAsync(id);
  await addr_fetch();
};

// ---------------------------------------------------
// RECIPE COMPONENTS
// ---------------------------------------------------
const rcService = new RecipeComponentService();
const prodService = new ProductService();
const rc_data = ref<IRecipeComponentEnriched[]>([]);
const rc_search = ref("");
const rc_show = ref(false);
const rc_mode = ref<"edit" | "create">("edit");
const rc_edit = ref<IRecipeComponentEnriched | null>(null);
const rc_create = ref<IRecipeComponent | null>(null);
const rc_empty: IRecipeComponent = { id: "", productRecipeId: "", componentProductId: "", amount: 0 };

const prod_all = ref<IProduct[]>([]);
const prod_loading = ref(false);
const rc_validation = ref("");
const rc_success = ref("");

const rc_fetch = async () => {
  const res = await rcService.getEnrichedRecipeComponents();
  rc_data.value = res.data ?? [];
};
const prod_fetch = async () => {
  prod_loading.value = true;
  try {
    const res = await prodService.getAllAsync();
    prod_all.value = res.data ?? [];
  } finally {
    prod_loading.value = false;
  }
};
const rc_filtered = computed(() => {
  const q = rc_search.value.trim().toLowerCase();
  if (!q) return rc_data.value;
  return rc_data.value.filter((rc) =>
    [
      rc.productRecipeName,
      rc.productRecipeCode,
      rc.componentProductName,
      rc.componentProductCode,
    ]
      .filter(Boolean)
      .some((v) => v!.toString().toLowerCase().includes(q))
  );
});
const rc_grouped = computed(() => {
  const m = new Map<string, { name: string; items: IRecipeComponentEnriched[] }>();
  for (const x of rc_filtered.value) {
    if (!m.has(x.productRecipeId)) m.set(x.productRecipeId, { name: x.productRecipeName, items: [] });
    m.get(x.productRecipeId)!.items.push(x);
  }
  return Array.from(m, ([key, val]) => ({ key, ...val }));
});

// multiselect helpers
type Opt = { id: string; label: string; raw: IProduct };
const toOpt = (p: IProduct): Opt => ({
  id: p.id,
  raw: p,
  label:
    `${p.name}` +
    (p.code ? ` (${p.code})` : "") +
    (p.unit ? ` · ${p.unit}` : "") +
    (p.volume ? ` · ${p.volume} ${p.volumeUnit ?? ""}` : "")
});
const rc_recipeOptions = computed<Opt[]>(() => prod_all.value.filter(p => !p.isComponent).map(toOpt));
const rc_componentOptions = computed<Opt[]>(() =>
  prod_all.value
    .filter(p => p.isComponent)
    .filter(p => !rc_selectedRecipe.value || p.id !== rc_selectedRecipe.value.id)
    .map(toOpt)
);
const rc_selectedRecipe = ref<Opt | null>(null);
const rc_selectedComponent = ref<Opt | null>(null);

watch(rc_selectedRecipe, (opt) => {
  if (!rc_active.value) return;
  rc_active.value.productRecipeId = opt?.id ?? "";
});
watch(rc_selectedComponent, (opt) => {
  if (!rc_active.value) return;
  rc_active.value.componentProductId = opt?.id ?? "";
});

const rc_hydrate = () => {
  const recId = rc_active.value?.productRecipeId ?? "";
  const compId = rc_active.value?.componentProductId ?? "";
  const rec = recId ? prod_all.value.find(p => p.id === recId && !p.isComponent) : undefined;
  const comp = compId ? prod_all.value.find(p => p.id === compId && p.isComponent) : undefined;
  rc_selectedRecipe.value = rec ? toOpt(rec) : null;
  rc_selectedComponent.value = comp ? toOpt(comp) : null;
};

const rc_openEdit = (row: IRecipeComponentEnriched) => {
  rc_edit.value = { ...row };
  rc_mode.value = "edit";
  rc_show.value = true;
  rc_hydrate();
};
const rc_openCreate = () => {
  rc_create.value = { ...rc_empty };
  rc_selectedRecipe.value = null;
  rc_selectedComponent.value = null;
  rc_mode.value = "create";
  rc_show.value = true;
};
const rc_active = computed<IRecipeComponent | null>({
  get() {
    return rc_mode.value === "edit"
      ? (rc_edit.value as unknown as IRecipeComponent | null)
      : rc_create.value;
  },
  set(v) {
    if (rc_mode.value === "edit") rc_edit.value = v as unknown as IRecipeComponentEnriched;
    else rc_create.value = v as IRecipeComponent;
  },
});
const rc_update = async () => {
  if (!rc_edit.value) return;
  await rcService.updateAsync(rc_edit.value);
  rc_show.value = false;
  await rc_fetch();
};
const rc_createFn = async () => {
  rc_validation.value = "";
  rc_success.value = "";
  if (!rc_create.value) return;
  const { id, ...rest } = rc_create.value;
  const cleaned = Object.fromEntries(
    Object.entries(rest).filter(([_, v]) => v !== null && v !== "")
  ) as unknown as IRecipeComponent;
  const res = await rcService.addAsync(cleaned);
  if (res.errors?.length) rc_validation.value = res.errors.join(", ");
  else {
    rc_success.value = "✅ Component added!";
    rc_show.value = false;
    await rc_fetch();
  }
};
const rc_remove = async (id: string) => {
  if (!confirm("Delete this component?")) return;
  await rcService.removeAsync(id);
  await rc_fetch();
};

// -------- initial load --------
onMounted(async () => {
  await Promise.all([at_fetch(), rs_fetch(), pc_fetch(), addr_fetch(), rc_fetch(), prod_fetch()]);
});

// kaardista iga taba oma otsingu-refile
const searchByTab: Record<TabKey, Ref<string>> = {
  actionTypes: at_search,
  reasons: rs_search,
  productCategories: pc_search,
  addresses: addr_search,
  recipeComponents: rc_search,
};

// v-model proxy aktiivse taba otsingule
const currentSearch = computed<string>({
  get: () => searchByTab[activeTab.value].value,
  set: (v) => { searchByTab[activeTab.value].value = v; },
});

// (valikuline) placeholderid tabade kaupa – kergem lugeda
const placeholderByTab: Record<TabKey, string> = {
  actionTypes: "Search by name",
  reasons: "Search by description",
  productCategories: "Search by name",
  addresses: "Search by name",
  recipeComponents: "Search by recipe/component",
};
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
          Settings
        </span>
      </h1>
      <div class="mt-4 mx-auto h-px w-128 max-w-full bg-gradient-to-r from-transparent via-neutral-500/40 to-transparent"></div>
      <p class="mt-3 text-sm text-neutral-400">Manage reference data & components — all in one place</p>
    </section>

    <!-- Tabs -->
    <nav
      class="mb-5 flex flex-wrap items-center justify-center gap-2
             rounded-2xl border-1 border-neutral-700/70 bg-neutral-900/40 p-2 backdrop-blur-xl">
      <button
        v-for="t in tabs"
        :key="t.key"
        @click="activeTab = t.key"
        class="inline-flex items-center justify-center rounded-xl px-3.5 py-2 text-sm font-semibold
               border-1 border-neutral-700 bg-white/5 text-neutral-200
               hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
        :class="{
          'bg-gradient-to-br from-cyan-500/20 via-cyan-400/15 to-transparent text-white shadow-[0_0_0_1px_rgba(34,211,238,0.25),0_8px_24px_rgba(0,0,0,0.35)]':
            activeTab === t.key
        }"
      >
        {{ t.label }}
      </button>
    </nav>

    <!-- Card container -->
    <section
      class="rounded-[16px] p-6 sm:p-8 bg-[rgba(25,25,25,0.4)] backdrop-blur-xl
             border-1 border-neutral-700 shadow-[inset_0_0_20px_rgba(255,255,255,0.03),_0_8px_24px_rgba(0,0,0,0.35)]">

      <!-- Toolbar -->
      <div class="mb-6 flex items-center justify-between gap-3 flex-wrap">
        <div class="relative">
          <input
            v-model="currentSearch"
            :placeholder="placeholderByTab[activeTab]"
            class="w-[300px] max-w-full appearance-none rounded-xl border-1 border-neutral-700 bg-neutral-900/70 text-white
         h-11 text-sm pl-10 pr-3 focus:outline-none focus:ring-2 focus:ring-cyan-400/30
         focus:border-neutral-500 transition shadow-inner shadow-black/30"
          />

          <i class="bi bi-search pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400"></i>
        </div>

        <button
          v-if="activeTab!=='recipeComponents'"
          @click="activeTab==='actionTypes' ? at_openCreate()
                 : activeTab==='reasons' ? rs_openCreate()
                 : activeTab==='productCategories' ? pc_openCreate()
                 : addr_openCreate()"
          class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                 border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                 shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                 hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
          <i class="bi bi-plus-lg opacity-90 group-hover:opacity-100"></i>
          <span>New</span>
        </button>

        <button
          v-else
          @click="rc_openCreate"
          class="group inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                 border-1 border-neutral-700 bg-gradient-to-br from-cyan-500/15 via-cyan-400/10 to-transparent text-cyan-200
                 shadow-[0_0_0_1px_rgba(34,211,238,0.25),_0_8px_24px_rgba(0,0,0,0.35)]
                 hover:from-cyan-400/25 hover:via-cyan-300/15 hover:text-white
                 focus:outline-none focus:ring-2 focus:ring-cyan-400/30 transition">
          <i class="bi bi-plus-lg opacity-90 group-hover:opacity-100"></i>
          <span>New Component</span>
        </button>
      </div>

      <!-- Grids -->
      <!-- Action Types -->
      <div v-if="activeTab==='actionTypes'" class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4 xl:grid-cols-5">
        <div v-for="row in at_filtered" :key="row.id"
             class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
                    shadow hover:bg-white/10 transition">
          <button
            class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                   border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                   hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
            @click="at_remove(row.id)" title="Remove">
            <i class="bi bi-trash3"></i>
          </button>
          <h3 class="text-lg font-semibold text-neutral-100 pr-10">{{ row.name }}</h3>
          <div class="mt-4 flex justify-end">
            <button class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                           border-1 border-neutral-700 bg-white/5 text-neutral-200
                           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                    @click="at_openEdit(row)">
              <i class="bi bi-pencil"></i> Edit
            </button>
          </div>
        </div>
      </div>

      <!-- Reasons -->
      <div v-else-if="activeTab==='reasons'" class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4 xl:grid-cols-5">
        <div v-for="row in rs_filtered" :key="row.id"
             class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
                    shadow hover:bg-white/10 transition">
          <button class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                         border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                         hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
                  @click="rs_remove(row.id)" title="Remove">
            <i class="bi bi-trash3"></i>
          </button>
          <h3 class="text-lg font-semibold text-neutral-100 pr-10">{{ row.description }}</h3>
          <div class="mt-4 flex justify-end">
            <button class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                           border-1 border-neutral-700 bg-white/5 text-neutral-200
                           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                    @click="rs_openEdit(row)">
              <i class="bi bi-pencil"></i> Edit
            </button>
          </div>
        </div>
      </div>

      <!-- Product Categories -->
      <div v-else-if="activeTab==='productCategories'" class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4 xl:grid-cols-5">
        <div v-for="row in pc_filtered" :key="row.id"
             class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
                    shadow hover:bg-white/10 transition">
          <button class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                         border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                         hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
                  @click="pc_remove(row.id)" title="Remove">
            <i class="bi bi-trash3"></i>
          </button>
          <h3 class="text-lg font-semibold text-neutral-100 pr-10">{{ row.name }}</h3>
          <div class="mt-4 flex justify-end">
            <button class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                           border-1 border-neutral-700 bg-white/5 text-neutral-200
                           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                    @click="pc_openEdit(row)">
              <i class="bi bi-pencil"></i> Edit
            </button>
          </div>
        </div>
      </div>

      <!-- Addresses -->
      <div v-else-if="activeTab==='addresses'" class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <div v-for="row in addr_filtered" :key="row.id"
             class="relative rounded-lg p-4 bg-neutral-900/60 border border-neutral-700
                    shadow hover:bg-white/10 transition">
          <button class="absolute right-2 top-2 inline-flex items-center justify-center rounded-full w-8 h-8
                         border-1 border-rose-400/50 bg-rose-500/10 text-rose-300
                         hover:bg-rose-500/15 hover:text-white focus:outline-none focus:ring-2 focus:ring-rose-400/30"
                  @click="addr_remove(row.id)" title="Remove">
            <i class="bi bi-trash3"></i>
          </button>
          <h3 class="text-lg font-semibold text-neutral-100 pr-10">{{ row.name }}</h3>
          <p class="text-sm text-neutral-400 mt-1">{{ row.fullAddress }}</p>
          <div class="mt-4 flex justify-end">
            <button class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                           border-1 border-neutral-700 bg-white/5 text-neutral-200
                           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                    @click="addr_openEdit(row)">
              <i class="bi bi-pencil"></i> Edit
            </button>
          </div>
        </div>
      </div>

      <!-- Recipe Components -->
      <div v-else class="grid gap-4 sm:grid-cols-1 lg:grid-cols-2 xl:grid-cols-3">
        <div v-for="g in rc_grouped" :key="g.key"
             class="rounded-lg p-4 bg-neutral-900/60 border border-neutral-700 shadow">
          <h3 class="text-lg font-semibold text-neutral-100 flex items-center gap-2">
            {{ g.name }} - 1 g / ml / l etc.
            <span class="inline-flex items-center justify-center min-w-[24px] h-[22px] px-2 rounded-full text-xs font-bold text-black
                         bg-gradient-to-r from-amber-400 to-orange-500 shadow">
              {{ g.items.length }}
            </span>
          </h3>

          <div class="mt-3 flex flex-wrap gap-2">
            <div v-for="item in g.items" :key="item.id"
                 class="relative inline-flex items-center gap-1.5 rounded-full border border-amber-400/30
                        bg-gradient-to-br from-amber-400/10 via-transparent to-transparent
                        px-2.5 py-1.5 text-sm text-neutral-100">
              <button
                class="inline-flex items-center justify-center w-5 h-5 rounded-full
                       border-1 border-rose-500/60 bg-rose-500/10 text-rose-300
                       hover:bg-rose-500/20 hover:text-white"
                @click.stop="rc_remove(item.id)" title="Remove">
                <X class="w-3.5 h-3.5" />
              </button>
              <span class="font-semibold">{{ item.componentProductName }}</span>
              <span class="opacity-60">•</span>
              <span class="font-semibold text-amber-300">{{ item.amount }} <span v-if="item.componentProductUnit">{{ item.componentProductUnit }}</span></span>
            </div>
          </div>

          <div class="mt-4 flex justify-end">
            <button class="inline-flex items-center gap-2 rounded-lg px-3 py-1.5 text-sm font-medium
                           border-1 border-neutral-700 bg-white/5 text-neutral-200
                           hover:bg-white/10 focus:outline-none focus:ring-2 focus:ring-white/10 transition"
                    @click="rc_openEdit(g.items[0])">
              <i class="bi bi-pencil"></i> Edit
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- MODALS (centered) -->
    <!-- ActionType -->
    <transition name="fade">
      <div v-if="at_show" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="at_show=false">
        <div class="w-full max-w-xl rounded-2xl border-1 border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ at_mode === 'edit' ? (at_edit?.name || 'Edit Action Type') : 'Create Action Type' }}
            </h2>
            <button class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-white/10 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white" @click="at_show=false">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6">
            <div>
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
              <input v-model="at_active!.name" type="text" class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/>
            </div>
          </div>

          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button v-if="at_mode==='edit'" @click="at_update" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Update</button>
            <button v-else @click="at_createFn" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Create</button>
            <button @click="at_show=false" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Cancel</button>
          </div>
        </div>
      </div>
    </transition>

    <!-- Reason -->
    <transition name="fade">
      <div v-if="rs_show" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="rs_show=false">
        <div class="w-full max-w-xl rounded-2xl border-1 border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ rs_mode === 'edit' ? (rs_edit?.description || 'Edit Reason') : 'Create Reason' }}
            </h2>
            <button class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-white/10 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white" @click="rs_show=false">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6">
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Description</label>
            <input v-model="rs_active!.description" type="text" class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/>
          </div>

          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button v-if="rs_mode==='edit'" @click="rs_update" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Update</button>
            <button v-else @click="rs_createFn" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Create</button>
            <button @click="rs_show=false" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Cancel</button>
          </div>
        </div>
      </div>
    </transition>

    <!-- Product Category -->
    <transition name="fade">
      <div v-if="pc_show" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="pc_show=false">
        <div class="w-full max-w-xl rounded-2xl border-1 border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ pc_mode === 'edit' ? (pc_edit?.name || 'Edit Product Category') : 'Create Product Category' }}
            </h2>
            <button class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-white/10 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white" @click="pc_show=false">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6">
            <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label>
            <input v-model="pc_active!.name" type="text" class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/>
          </div>

          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button v-if="pc_mode==='edit'" @click="pc_update" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Update</button>
            <button v-else @click="pc_createFn" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Create</button>
            <button @click="pc_show=false" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Cancel</button>
          </div>
        </div>
      </div>
    </transition>

    <!-- Address -->
    <transition name="fade">
      <div v-if="addr_show" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="addr_show=false">
        <div class="w-full max-w-xl rounded-2xl border-1 border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ addr_mode === 'edit' ? (addr_edit?.name || 'Edit Address') : 'Create Address' }}
            </h2>
            <button class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-white/10 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white" @click="addr_show=false">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Name</label><input v-model="addr_active!.name" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Street</label><input v-model="addr_active!.streetName" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Building Nr</label><input v-model.number="addr_active!.buildingNr" type="number" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Unit Nr</label><input v-model.number="addr_active!.unitNr" type="number" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Postal Code</label><input v-model="addr_active!.postalCode" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">City</label><input v-model="addr_active!.city" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Province</label><input v-model="addr_active!.province" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
            <div><label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Country</label><input v-model="addr_active!.country" type="text" class="w-full rounded-xl border border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/></div>
          </div>

          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button v-if="addr_mode==='edit'" @click="addr_update" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Update</button>
            <button v-else @click="addr_createFn" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Create</button>
            <button @click="addr_show=false" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Cancel</button>
          </div>
        </div>
      </div>
    </transition>

    <!-- Recipe Components -->
    <transition name="fade">
      <div v-if="rc_show" class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/60 p-4" @click.self="rc_show=false">
        <div class="w-full max-w-2xl rounded-2xl border-1 border-white/10 bg-neutral-950/90 backdrop-blur-xl p-6 sm:p-8 shadow-[0_20px_60px_rgba(0,0,0,0.6)]">
          <div class="flex items-start justify-between gap-4">
            <h2 class="text-2xl font-bold tracking-tight text-neutral-100">
              {{ rc_mode === 'edit' ? 'Edit Recipe Component' : 'Create Recipe Component' }}
            </h2>
            <button class="inline-flex items-center justify-center w-9 h-9 rounded-xl border-1 border-white/10 bg-white/5 text-neutral-300 hover:bg-white/10 hover:text-white" @click="rc_show=false">
              <i class="bi bi-x-lg"></i>
            </button>
          </div>

          <div class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Recipe Product</label>
              <Multiselect
                v-model="rc_selectedRecipe"
                :options="rc_recipeOptions"
                track-by="id"
                label="label"
                :loading="prod_loading"
                :searchable="true"
                :close-on-select="true"
                placeholder="Select recipe product (not a component)"
                class="multiselect-dark w-full"
              />
            </div>
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Component Product</label>
              <Multiselect
                v-model="rc_selectedComponent"
                :options="rc_componentOptions"
                track-by="id"
                label="label"
                :loading="prod_loading"
                :searchable="true"
                :close-on-select="true"
                placeholder="Select component product"
                class="multiselect-dark w-full"
              />
            </div>
            <div class="sm:col-span-2">
              <label class="mb-2 block text-xs uppercase tracking-wide text-neutral-400">Amount</label>
              <input v-model.number="rc_active!.amount" type="number" min="0" step="0.01"
                     class="w-full rounded-xl border-1 border-neutral-700 bg-neutral-900/70 px-4 h-11 text-sm text-white focus:ring-2 focus:ring-cyan-400/20 focus:border-cyan-400/40"/>
            </div>
          </div>

          <div class="mt-6 flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button v-if="rc_mode==='edit'" @click="rc_update" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Update</button>
            <button v-else @click="rc_createFn" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Create</button>
            <button @click="rc_show=false" class="rounded-xl border-1 border-neutral-700 bg-white/5 px-6 h-11 text-base font-medium text-neutral-200 hover:bg-white/10">Cancel</button>
          </div>

          <p v-if="rc_validation" class="mt-3 text-rose-400 text-center font-medium">{{ rc_validation }}</p>
          <p v-if="rc_success" class="mt-3 text-emerald-400 text-center font-medium">{{ rc_success }}</p>
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
              See leht võimaldab hallata rakenduse <strong>põhiandmeid</strong> (Action Types, Reasons, Product Categories, Addresses)
              ning <strong>Recipe Components</strong> seoseid. Kasuta ülariba sakke, et liikuda andmekogude vahel.
            </p>

            <ul class="list-disc pl-6 space-y-2 text-neutral-300">
              <li>
                <strong>Otsing:</strong> tööriistariba otsing filtreerib <em>aktiivse saki</em> elemente (nt nime või kirjelduse järgi).
              </li>
              <li>
                <strong>Uus kirje:</strong> klõpsa <em>New</em> (või sakis <em>Recipe Components</em> nupul <em>New Component</em>),
                täida vorm ja salvesta.
              </li>
              <li>
                <strong>Muutmine:</strong> kaardi nupust <em>Edit</em> avad vormi olemasoleva kirje muutmiseks.
              </li>
              <li>
                <strong>Kustutamine:</strong> prügikasti ikoon kustutab kirje pärast kinnitust. Seda toimingut ei saa tagasi võtta.
              </li>
              <li>
                <strong>Recipe Components:</strong> siin seod retseptitoote (Recipe Product) konkreetsete
                komponenditoodetega (Component Product) ja määrad <em>amount</em> (näiteks 1 retseptiühik = X g/ml komponenti).
              </li>
              <li>
                <strong>Aadressid:</strong> Addresses sakis haldad aadressi põhiandmeid (nimi, tänav, maja/üksus, indeks, linn jm).
              </li>
            </ul>

            <p class="text-neutral-400 text-sm">
              Nipp: modaali saab sulgeda taustale klõpsates või ülanurga sulgemisnupust. Kiiremaks
              navigeerimiseks vaheta sakke ja kasuta otsingut enne uue kirje lisamist.
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
/* jäta <style scoped> alles */
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
