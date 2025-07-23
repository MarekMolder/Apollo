<script setup>
import PendingActionsCard from "@/components/PendingActionsCard.vue";
import ProblematicProducts from "@/components/ProblematicProducts.vue";
import LowestStockProductsCard from "@/components/LowestStockProductsCard.vue";
import TopUsersByRemoveCard from "@/components/TopUsersByRemoveCard.vue";
import TotalInventoryWorthCard from "@/components/TotalInventoryWorthCard.vue";
import ProductQuickSearchCard from "@/components/ProductQuickSearchCard.vue";
import UserListCard from "@/components/UserListCard.vue";
</script>

<template>
  <div class="dashboard-grid">
    <!-- RIDA 1 -->
    <ProductQuickSearchCard class="card card-info" />
    <TotalInventoryWorthCard class="card card-focus wide" />
    <PendingActionsCard class="card card-warning" />
    <ProblematicProducts class="card card-danger" />


    <!-- RIDA 2 -->
    <TopUsersByRemoveCard class="card card-blue" />
    <UserListCard class="card card-default" />
    <LowestStockProductsCard class="card card-warning" />
  </div>
</template>

<style scoped>
.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2rem;
  padding: 3rem 4rem;
}

/* Kaardid */
.card {
  backdrop-filter: blur(12px);
  background: rgba(20, 20, 20, 0.7);
  padding: 1.6rem;
  border-radius: 16px;
  box-shadow: 0 8px 24px rgba(255, 255, 255, 0.05);
  display: flex;
  flex-direction: column;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.04);
  min-width: 0;
}

.card:hover {
  transform: scale(1.02);
  box-shadow: 0 12px 28px rgba(255, 170, 51, 0.2);
}

.card.wide {
  grid-column: span 2;
}

/* Värvid */
.card-danger { border-left: 5px solid #e74c3c; }
.card-warning { border-left: 5px solid #ffaa33; }
.card-info { border-left: 5px solid #3498db; }
.card-blue { border-left: 5px solid #1abc9c; }
.card-focus { border-left: 5px solid #9b59b6; }
.card-default { border-left: 5px solid #666; }

/* Responsive */
@media (max-width: 1024px) {
  .dashboard-grid {
    grid-template-columns: 1fr;
    padding: 2rem;
  }

  .card.wide {
    grid-column: span 1;
  }
}
</style>
