<script setup lang="ts">
import { RouterLink } from "vue-router";
import { useUserDataStore } from "@/stores/userDataStore";
import { computed, ref } from "vue";

const store = useUserDataStore();
const isAdmin = computed(() => {
  const roles = store.roles;

  return Array.isArray(roles)
    ? roles.includes("admin") || roles.includes("manager")   // massiiv
    : roles === "admin" || roles === "manager";              // üksik string
});

// kontrollime hoverit
const isOpen = ref(false);
</script>

<template>
  <aside class="sidebar" :class="{ open: isOpen }" @mouseenter="isOpen = true" @mouseleave="isOpen = false">
    <div class="sidebar-logo">🎬</div>

    <nav class="sidebar-nav">
      <RouterLink to="/createaction" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-plus-circle"></i>
        <span>{{ $t('Create') }}</span>
      </RouterLink>

      <RouterLink v-if="isAdmin" to="/home" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-speedometer2"></i>
        <span>{{ $t('Dashboard') }}</span>
      </RouterLink>

      <RouterLink v-if="isAdmin" to="/actionrequest" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-bell"></i>
        <span>{{ $t('Requests') }}</span>
      </RouterLink>

      <RouterLink v-if="isAdmin" to="/inventories" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-building"></i>
        <span>{{ $t('Inventory') }}</span>
      </RouterLink>

      <RouterLink v-if="isAdmin" to="/products" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-box"></i>
        <span>{{ $t('Products') }}</span>
      </RouterLink>

      <RouterLink v-if="isAdmin" to="/suppliers" class="sidebar-link" :class="{ open: isOpen }">
        <i class="bi bi-telephone"></i>
        <span>{{ $t('Contacts') }}</span>
      </RouterLink>
    </nav>
  </aside>
</template>

<style scoped>
.sidebar {
  position: fixed;
  top: 56px;
  left: 0;
  height: calc(100vh - 56px);
  background-color: #111;
  width: 64px;
  transition: width 0.3s ease;
  overflow-x: hidden;
  z-index: 1000;
  display: flex;
  flex-direction: column;
  padding: 1rem 0;
  border-right: 1px solid #2a2a2a;
}

.sidebar.open {
  width: 150px;
}

.sidebar-logo {
  color: orange;
  font-size: 1.8rem;
  text-align: center;
  margin-bottom: 2rem;
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.sidebar-link {
  display: flex;
  align-items: center;
  color: #ddd;
  text-decoration: none;
  padding: 0.75rem 1rem;
  transition: background 0.2s ease, color 0.2s ease;
  font-size: 0.95rem;
  white-space: nowrap;
  position: relative;
}

.sidebar-link i {
  font-size: 1.2rem;
  width: 24px;
  text-align: center;
}

.sidebar-link span {
  opacity: 0;
  margin-left: 1rem;
  transition: opacity 0.3s ease;
}

.sidebar.open .sidebar-link span {
  opacity: 1;
}

.sidebar-link:hover {
  background-color: rgba(255, 165, 0, 0.1);
  color: orange;
}
</style>
